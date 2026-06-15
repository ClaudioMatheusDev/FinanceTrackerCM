using Microsoft.AspNetCore.Mvc;
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
        var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email, TenantId = dto.TenantId };
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
        var refresh = _tokenService.CreateRefreshToken(user.Id, int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"]));
        _db.RefreshTokens.Add(refresh);
        await _db.SaveChangesAsync();

        return Ok(new { accessToken = access, refreshToken = refresh.Token });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshDto dto)
    {
        var rt = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == dto.RefreshToken && !x.Revoked);
        if (rt == null || rt.ExpiresAt < DateTime.UtcNow) return Unauthorized();

        var user = await _userManager.FindByIdAsync(rt.UserId.ToString());
        if (user == null) return Unauthorized();

        rt.Revoked = true; // rotate
        var newRefresh = _tokenService.CreateRefreshToken(user.Id, int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"]));
        _db.RefreshTokens.Add(newRefresh);
        await _db.SaveChangesAsync();

        var access = _tokenService.CreateAccessToken(user);
        return Ok(new { accessToken = access, refreshToken = newRefresh.Token });
    }
}
} 