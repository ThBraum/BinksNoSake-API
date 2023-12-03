using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BinksNoSake.Persistence.Persistence;
public class TokenPersit : ITokenPersit
{
    private readonly BinksNoSakeContext _context;
    public TokenPersit(BinksNoSakeContext context)
    {
        _context = context;
    }
    
    public void DeleteRefreshToken(string username, string refreshToken)
    {
        var item = _context.RefreshTokens.FirstOrDefault(x => x.Username == username && x.RefreshToken == refreshToken);
        _context.RefreshTokens.Remove(item);
    }

    public async Task<string> GetRefreshToken(string username)
    {
        IQueryable<RefreshTokens> query = _context.RefreshTokens.AsNoTracking();

        query = query.Where(r => r.Username == username);

        return await query.Select(r => r.RefreshToken).FirstOrDefaultAsync();
    }
}