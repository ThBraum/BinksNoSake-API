using BinksNoSake.Application.Dtos;
using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Pagination;

namespace BinksNoSake.Application.Contratos;
public interface IPirataService
{
    Task<PirataDto> AddPirata(PirataDto model);
    Task<PirataDto> GetPirataByIdAsync(int pirataId);
    Task<PirataDto> UpdatePirata(int pirataId, PirataDto model);
    Task<bool> DeletePirata(int pirataId);
    Task<PageList<PirataDto>> GetAllPiratasAsync(PageParams pageParams);
}