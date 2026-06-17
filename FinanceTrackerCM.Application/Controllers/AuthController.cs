using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using FinanceTrackerCM.Application.Users;
using FinanceTrackerCM.Application.DTOs;
using FinanceTrackerCM.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinanceTrackerCM.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IAppDbContext _db;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            IAppDbContext db,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _db = db;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // Ao registrar, não aceite TenantId do cliente. Por padrão deixamos Guid.Empty;
            // administradores deverão associar o usuário a um tenant posteriormente.
            var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email, TenantId = Guid.Empty };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return Unauthorized();

            var pw = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!pw) return Unauthorized();

            var access = _tokenService.CreateAccessToken(user);
            var (refresh, raw) = _tokenService.CreateRefreshToken(user.Id, int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7"));
            _db.RefreshTokens.Add(refresh);
            await _db.SaveChangesAsync();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Strict,
                Expires = refresh.ExpiresAt
            };
            Response.Cookies.Append("ft_refresh", raw, cookieOptions);

            return Ok(new { accessToken = access });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshDto dto)
        {
            // Prefer cookie; fall back to body if provided
            var incoming = string.Empty;
            if (Request.Cookies.ContainsKey("ft_refresh")) incoming = Request.Cookies["ft_refresh"]!;
            if (string.IsNullOrWhiteSpace(incoming) && !string.IsNullOrWhiteSpace(dto.RefreshToken)) incoming = dto.RefreshToken;

            if (string.IsNullOrWhiteSpace(incoming)) return Unauthorized();

            var hash = _tokenService.ComputeRefreshTokenHash(incoming);
            var rt = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == hash && !x.Revoked);
            if (rt == null || rt.ExpiresAt < DateTime.UtcNow) return Unauthorized();

            var user = await _userManager.FindByIdAsync(rt.UserId.ToString());
            if (user == null) return Unauthorized();

            rt.Revoked = true; // rotate
            var (newRefresh, newRaw) = _tokenService.CreateRefreshToken(user.Id, int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7"));
            _db.RefreshTokens.Add(newRefresh);
            await _db.SaveChangesAsync();

            // replace cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Strict,
                Expires = newRefresh.ExpiresAt
            };
            Response.Cookies.Append("ft_refresh", newRaw, cookieOptions);

            var access = _tokenService.CreateAccessToken(user);
            return Ok(new { accessToken = access });
        }
    }
}