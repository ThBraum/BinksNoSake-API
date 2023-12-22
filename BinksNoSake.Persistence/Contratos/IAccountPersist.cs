using BinksNoSake.Domain.Identity;

namespace BinksNoSake.Persistence.Contratos;
public interface IAccountPersist : IGeralPersist
{
    Task<IEnumerable<Account>> GetUsersAsync();
    Task<Account> GetUserByIdAsync(int id);
    Task<Account> GetUserByUsernameAsync(string username);
    Task<Account> GetUserByEmailAsync(string email);
}