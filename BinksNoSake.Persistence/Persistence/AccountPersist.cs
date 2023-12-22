using BinksNoSake.Domain.Identity;
using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BinksNoSake.Persistence.Persistence;
public class AccountPersist : GeralPersist, IAccountPersist
{
    private readonly BinksNoSakeContext _context;
    public AccountPersist(BinksNoSakeContext context) : base(context)
    {
        _context = context;

    }

    public Task<Account> GetUserByEmailAsync(string email)
    {
        return _context.Users.AsNoTracking().OrderBy(u => u.Email)
            .SingleOrDefaultAsync(u => u.Email.ToLower() == email.ToLower()) ?? null;
    }

    public async Task<Account> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id) ?? null;
    }

    public async Task<Account> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.AsNoTracking().OrderBy(u => u.UserName)
            .SingleOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower()) ?? null;
    }

    public async Task<IEnumerable<Account>> GetUsersAsync()
    {
        IQueryable<Account> query = _context.Users
            .OrderBy(u => u.UserName);
        
        return await query.ToListAsync();
    }
}