using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Pagination;

namespace BinksNoSake.Persistence.Contratos;
public interface ICapitaoPersist
{
    Task<PageList<CapitaoModel>> GetAllCapitaesAsync(PageParams pageParams);
    Task<CapitaoModel> GetCapitaoByIdAsync(int capitaoId);
    Task<CapitaoModel> AddCapitaoWithExistingPiratasAsync(CapitaoModel capitao, List<int> piratas);
}