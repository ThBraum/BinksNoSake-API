using BinksNoSake.Application.Dtos;
using BinksNoSake.Domain.Models;

namespace BinksNoSake.Application.Contratos;
public interface IPirataService
{
    Task<PirataDto> AddPirata(PirataDto model);
    Task<PirataDto> GetPirataByIdAsync(int pirataId);
    Task<PirataDto> UpdatePirata(int pirataId, PirataDto model);
    Task<bool> DeletePirata(int pirataId);
    Task<PirataDto[]> GetAllPiratasAsync();
    Task<PirataDto[]> GetAllPiratasByNomeAsync(string nome);
}