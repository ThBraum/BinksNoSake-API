using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BinksNoSake.Persistence.Persistence;

public class CapitaoPersist : ICapitaoPersist
{
    private readonly BinksNoSakeContext _context;
    public CapitaoPersist(BinksNoSakeContext context)
    {
        _context = context;
    }

    public async Task<CapitaoModel[]> GetAllCapitaesAsync()
    {
        IQueryable<CapitaoModel> query = _context.Capitaes.AsNoTracking()
            .Include(c => c.Piratas)
            .Include(c => c.Timoneiro);
        
        query.OrderBy(p => p.CapitaoId);

        return await query.ToArrayAsync();    
    }

    public async Task<CapitaoModel[]> GetAllCapitaesByNomeAsync(string nome)
    {
        IQueryable<CapitaoModel> query = _context.Capitaes.AsNoTracking()
            .Include(c => c.Piratas)
            .Include(c => c.Timoneiro);
        
        query.Where(c => c.Nome.ToLower().Contains(nome.ToLower())).OrderBy(c => c.CapitaoId);

        return await query.ToArrayAsync();
    }

    public async Task<CapitaoModel> GetCapitaoByIdAsync(int capitaoId)
    {
        IQueryable<CapitaoModel> query = _context.Capitaes.AsNoTracking()
            .Include(c => c.Piratas)
            .Include(c => c.Timoneiro);
        
        query = query.Where(c => c.CapitaoId == capitaoId).OrderBy(c => c.CapitaoId);

        return await query.FirstOrDefaultAsync();
    }
}
