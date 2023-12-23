using BinksNoSake.Application.Dtos;
using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Pagination;

namespace BinksNoSake.Application.Contratos;
public interface ICapitaoService
{
    Task<CapitaoDto> AddCapitao(CapitaoDto model);
    Task<CapitaoDto> GetCapitaoByIdAsync(int capitaoId);
    Task<CapitaoDto> UpdateCapitao(int capitaoId, CapitaoDto model);
    Task<bool> DeleteCapitao(int capitaoId);
    Task<PageList<CapitaoDto>> GetAllCapitaesAsync(PageParams pageParams);
}