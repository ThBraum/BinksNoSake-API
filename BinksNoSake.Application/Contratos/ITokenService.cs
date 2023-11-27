using BinksNoSake.Application.Dtos;

namespace BinksNoSake.Application.Contratos;
public interface ITokenService
{
    Task<string> CreateToken(AccountUpdateDto accountUpdateDto);
}