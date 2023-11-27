using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;
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
        var capitaoExistente = await _context.Capitaes.FindAsync(capitao.Id);
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

    public async Task<PirataModel[]> GetAllPiratasAsync()
    {
        IQueryable<PirataModel> query = _context.Piratas.AsNoTracking()
            .Include(p => p.Capitao)
            .Include(p => p.Navios);
        
        query.OrderBy(p => p.Id);

        return await query.ToArrayAsync();
    }

    public async Task<PirataModel[]> GetAllPiratasByNomeAsync(string nome)
    {
        IQueryable<PirataModel> query = _context.Piratas.AsNoTracking()
            .Include(p => p.Capitao)
            .Include(p => p.Navios);
        
        query.Where(p => p.Nome.ToLower().Contains(nome.ToLower())).OrderBy(p => p.Id);

        return await query.ToArrayAsync();
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