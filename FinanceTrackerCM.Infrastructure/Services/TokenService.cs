using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using FinanceTrackerCM.Application.Interfaces;
using FinanceTrackerCM.Application.Users;

namespace FinanceTrackerCM.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config; 
    public TokenService(IConfiguration config) => _config = config;
/// <summary>
/// Cria um token de acesso JWT para o usuário fornecido, incluindo claims como ID,
/// nome de usuário, email e TenantId. O token é assinado usando HMAC SHA256 e tem uma expiração configurada.
/// </summary> <param name="user">O usuário para o qual o token de acesso será criado.</param>
/// <returns>Uma string representando o token de acesso JWT criado para o usuário.</returns>
    public string CreateAccessToken(ApplicationUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException("Chave JWT não configurada")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim("TenantId", user.TenantId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:AccessTokenExpirationMinutes"] ?? throw new InvalidOperationException("Chave JWT de expiração do token de acesso não configurada"))),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
/// <summary>
///     Cria um token de atualização (refresh token) para o usuário especificado, gerando um token aleatório e 
/// definindo sua expiração com base no número de dias fornecido. O token é associado ao ID do usuário para permitir a renovação do token de acesso quando necessário.
/// </summary>
/// <param name="userId"></param>
/// <param name="days"></param>
/// <returns></returns>
    public (RefreshToken RefreshToken, string RawToken) CreateRefreshToken(Guid userId, int days)
    {
        var raw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var hash = ComputeRefreshTokenHash(raw);
        var refresh = new RefreshToken
        {
            UserId = userId,
            Token = hash,
            ExpiresAt = DateTime.UtcNow.AddDays(days)
        };
        return (refresh, raw);
    }

    public string ComputeRefreshTokenHash(string token)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash); // .NET 5+ helper
    }
}
