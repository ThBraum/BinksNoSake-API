using BinksNoSake.Domain.Models;

namespace BinksNoSake.Persistence.Contratos;
public interface ICapitaoPersist
{
    Task<CapitaoModel[]> GetAllCapitaesAsync();
    Task<CapitaoModel> GetCapitaoByNomeAsync(string nome);
    Task<CapitaoModel> GetCapitaoByIdAsync(int capitaoId);
}