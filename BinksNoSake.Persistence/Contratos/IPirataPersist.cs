using BinksNoSake.Domain.Models;

namespace BinksNoSake.Persistence.Contratos;
public interface IPirataPersist
{
    Task<PirataModel[]> GetAllPiratasAsync();
    Task<PirataModel[]> GetAllPiratasByNomeAsync(string nome);
    Task<PirataModel> GetPirataByIdAsync(int pirataId);
    Task<PirataModel> AddPirataWithExistingCapitaoAsync(PirataModel pirata, CapitaoModel capitao);
}