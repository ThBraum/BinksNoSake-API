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
        IQueryable<RefreshTokens> query = _context.RefreshTokens.AsNoTracking();

        query = query.Where(r => r.Username == username && r.RefreshToken == refreshToken);

        _context.RefreshTokens.Remove(query.FirstOrDefault());
    }

    public async Task<string> GetRefreshToken(string username)
    {
        IQueryable<RefreshTokens> query = _context.RefreshTokens.AsNoTracking();

        query = query.Where(r => r.Username == username);

        return await query.Select(r => r.RefreshToken).FirstOrDefaultAsync();
    }
}