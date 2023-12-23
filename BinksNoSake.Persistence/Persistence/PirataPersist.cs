using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;
using BinksNoSake.Persistence.Pagination;
using Microsoft.EntityFrameworkCore;

namespace BinksNoSake.Persistence.Persistence;
public class PirataPersist : IPirataPersist
{
    private readonly BinksNoSakeContext _context;
    public PirataPersist(BinksNoSakeContext context)
    {
        _context = context;
    }

    public async Task<PirataModel> AddPirataWithExistingCapitaoAsync(PirataModel pirata, CapitaoModel capitao)
    {
        var capitaoExistente = await _context.Capitaes.Include(c => c.Timoneiro)
                                        .FirstOrDefaultAsync(c => c.Id == capitao.Id);
                                        
        if (capitaoExistente != null)
        {
            pirata.Capitao = capitaoExistente;
        }
        else
        {
            pirata.Capitao = capitao;
        }

        _context.Piratas.Add(pirata);

        return pirata;
    }

    public async Task<PageList<PirataModel>> GetAllPiratasAsync(PageParams pageParams)
    {
        IQueryable<PirataModel> query = _context.Piratas.AsNoTracking()
            .Include(p => p.Capitao)
            .Include(p => p.Navios);

        query = query.Where(p => p.Nome.ToLower().Contains(pageParams.Term.ToLower()) ||
                                p.Funcao.ToLower().Contains(pageParams.Term.ToLower()) ||
                                p.Objetivo.ToLower().Contains(pageParams.Term.ToLower())).OrderBy(p => p.Id);

        return await PageList<PirataModel>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
    }


    public async Task<PirataModel> GetPirataByIdAsync(int pirataId)
    {
        IQueryable<PirataModel> query = _context.Piratas.AsNoTracking()
            .Include(p => p.Capitao)
            .Include(p => p.Navios);

        query = query.Where(p => p.Id == pirataId).OrderBy(p => p.Id);

        return await query.FirstOrDefaultAsync();
    }

}