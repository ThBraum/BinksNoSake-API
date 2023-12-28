using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using BinksNoSake.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using BinksNoSake.Persistence.Contratos;
using BinksNoSake.Domain.Models;

namespace BinksNoSake.Application.Services;
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<Account> _userManager;
    private readonly IMapper _mapper;
    private readonly SymmetricSecurityKey _key;
    private readonly ITokenPersit _tokenPersit;
    private readonly IGeralPersist _geralPersist;
    public TokenService(IConfiguration configuration, UserManager<Account> userManager, IMapper mapper, ITokenPersit tokenPersit, IGeralPersist geralPersist)
    {
        _geralPersist = geralPersist;
        _tokenPersit = tokenPersit;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
    }

    public async Task<string> CreateToken(AccountUpdateDto accountUpdateDto)
    {
        var user = _mapper.Map<Account>(accountUpdateDto);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescription);

        return tokenHandler.WriteToken(token);
    }

    public async Task<string> CreateTokenEnumerator(IEnumerable<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = creds
        };

        var token = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(token);
    }

    public async Task<string> GenereteRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _key,
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase)) throw new SecurityTokenException("Token inválido!");

        return principal;
    }

    public List<(string, string)> _refreshTokens = new();

    public TimeSpan RefreshTokenExpiration { get; } = TimeSpan.FromDays(1);

    public async Task<RefreshTokenDto> SaveRefreshToken(string username, string refreshToken)
    {
        try
        {
            _geralPersist.Add<RefreshTokens>(new RefreshTokens
            {
                Username = username,
                RefreshToken = refreshToken
            });

            if (await _geralPersist.SaveChangesAsync())
            {
                var oldRefreshTokens = await _tokenPersit.GetRefreshToken(username);
                if (oldRefreshTokens != null)
                {
                    _tokenPersit.DeleteRefreshToken(username, oldRefreshTokens);
                }
                return _mapper.Map<RefreshTokenDto>(new RefreshTokens
                {
                    Username = username,
                    RefreshToken = refreshToken
                });
            }
            return null;
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    public async Task<string> GetRefreshToken(string username)
    {
        try
        {
            return await _tokenPersit.GetRefreshToken(username);
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    public async Task<bool> DeleteRefreshToken(string username, string refreshToken)
    {
        try
        {
            _tokenPersit.DeleteRefreshToken(username, refreshToken);

            if (await _geralPersist.SaveChangesAsync())
            {
                return true;
            }
            return false;
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    public Task<bool> DeleteExpiredRefreshToken()
    {
        try
        {
            var expiredRefreshTokens = _refreshTokens.Where(x => DateTime.Parse(x.Item2) < DateTime.UtcNow).ToList();

            foreach (var expiredRefreshToken in expiredRefreshTokens)
            {
                var username = expiredRefreshToken.Item1;
                var refreshToken = expiredRefreshToken.Item2;

                _tokenPersit.DeleteRefreshToken(username, refreshToken);
                _refreshTokens.Remove(expiredRefreshToken);
            }
            return Task.FromResult(true);
        }
        catch (System.Exception e)
        {
            throw new Exception($"Erro ao excluir Refresh Toekns expirados: {e.Message}");
        }
    }
}