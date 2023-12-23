using System.Security.Claims;
using BinksNoSake.Application.Dtos;

namespace BinksNoSake.Application.Contratos;
public interface ITokenService
{
    Task<string> CreateToken(AccountUpdateDto accountUpdateDto);
    Task<string> CreateTokenEnumerator(IEnumerable<Claim> claims);
    Task<string> GenereteRefreshToken();
    Task<bool> DeleteExpiredRefreshToken();
    TimeSpan RefreshTokenExpiration { get; }
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    Task<RefreshTokenDto> SaveRefreshToken(string username, string token);
    Task<string> GetRefreshToken(string username);
    Task<bool> DeleteRefreshToken(string username, string refreshToken);
}