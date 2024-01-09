using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;
using BinksNoSake.Persistence.Pagination;
using Microsoft.EntityFrameworkCore;

namespace BinksNoSake.Persistence.Persistence;

public class CapitaoPersist : ICapitaoPersist
{
    private readonly BinksNoSakeContext _context;
    public CapitaoPersist(BinksNoSakeContext context)
    {
        _context = context;
    }

    public async Task<CapitaoModel> AddCapitaoWithExistingPiratasAsync(CapitaoModel capitao, List<int> pirataIds)
    {
        var piratasExistente = await _context.Piratas
            .Include(p => p.Capitao)
            .Where(p => pirataIds.Contains(p.Id))
            .ToListAsync();

        foreach (var pirataExistente in piratasExistente)
        {
            pirataExistente.Capitao = capitao;
            capitao.Piratas.Add(pirataExistente);
        }

        _context.Capitaes.Add(capitao);

        return capitao;
    }
    public async Task<PageList<CapitaoModel>> GetAllCapitaesAsync(PageParams pageParams)
    {
        IQueryable<CapitaoModel> query = _context.Capitaes.AsNoTracking()
            .Include(c => c.Piratas)
            .Include(c => c.Timoneiro);

        query = query.Where(c => c.Nome.ToLower().Contains(pageParams.Term.ToLower())).OrderBy(c => c.Id);

        return await PageList<CapitaoModel>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
    }

    public async Task<CapitaoModel> GetCapitaoByIdAsync(int capitaoId)
    {
        IQueryable<CapitaoModel> query = _context.Capitaes.AsNoTracking()
            .Include(c => c.Piratas)
            .Include(c => c.Timoneiro);

        query = query.Where(c => c.Id == capitaoId).OrderBy(c => c.Id);

        return await query.FirstOrDefaultAsync();
    }
}
