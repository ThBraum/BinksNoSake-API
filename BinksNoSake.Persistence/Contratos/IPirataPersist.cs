using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Pagination;

namespace BinksNoSake.Persistence.Contratos;
public interface IPirataPersist
{
    Task<PageList<PirataModel>> GetAllPiratasAsync(PageParams pageParams);
    Task<PirataModel> GetPirataByIdAsync(int pirataId);
    Task<PirataModel> AddPirataWithExistingCapitaoAsync(PirataModel pirata, CapitaoModel capitao);
}