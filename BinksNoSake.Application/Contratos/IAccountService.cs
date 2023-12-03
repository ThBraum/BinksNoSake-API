using BinksNoSake.Application.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BinksNoSake.Application.Contratos;
public interface IAccountService
{
    Task<bool> UserExists(string username);
    Task<AccountUpdateDto> GetUserByUsernameAsync(string username);
    Task<SignInResult> CheckUserPasswordAsync(AccountUpdateDto accountUpdateDto, string password);
    Task<AccountDto> CreateAccountAsync(AccountDto userDto);
    Task<AccountUpdateDto> UpdateAccount(AccountUpdateDto accountUpdateDto);
    void DeleteImage(int userId, string imageName);
    Task<string> SaveImage(IFormFile image);
}