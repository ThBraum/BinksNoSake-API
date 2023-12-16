using AutoMapper;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;
using BinksNoSake.Persistence.Pagination;

namespace BinksNoSake.Application.Services;
public class PirataService : IPirataService
{
    private readonly IGeralPersist _geralPersist;
    private readonly IPirataPersist _pirataPersist;
    private readonly IMapper _mapper;
    private readonly ICapitaoPersist _capitaoPersist;
    public PirataService(IGeralPersist geralPersist, IPirataPersist pirataPersist, ICapitaoPersist capitaoPersist, IMapper mapper)
    {
        _capitaoPersist = capitaoPersist;
        _mapper = mapper;
        _pirataPersist = pirataPersist;
        _geralPersist = geralPersist;
    }

    public async Task<PirataDto> AddPirata(PirataDto model)
    {
        try
        {
            if (model.Capitao != null)
            {
                model.Capitao = await GetCapitaoByIdOrNome(model.Capitao);
            }
            if (model.CapitaoId != null)
            {
                model.Capitao = await GetCapitaoId(model, model.Capitao);
            }

            var pirata = _mapper.Map<PirataModel>(model);

            if (model.Capitao != null)
            {
                await _pirataPersist.AddPirataWithExistingCapitaoAsync(pirata, pirata.Capitao);
            }
            else
            {
                _geralPersist.Add<PirataModel>(pirata);
            }

            if (await _geralPersist.SaveChangesAsync())
            {
                return _mapper.Map<PirataDto>(pirata);
            }

            return null;
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> DeletePirata(int pirataId)
    {
        try
        {
            var pirata = await _pirataPersist.GetPirataByIdAsync(pirataId);
            if (pirata == null) throw new Exception("Pirata não encontrado.");
            _geralPersist.Delete<PirataModel>(pirata);
            if (await _geralPersist.SaveChangesAsync()) return true;
            return false;
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }

    }

    public async Task<PageList<PirataDto>> GetAllPiratasAsync(PageParams pageParams)
    {
        try
        {
            var piratas = await _pirataPersist.GetAllPiratasAsync(pageParams);
            if (piratas == null) return null;
            var resultado = _mapper.Map<PageList<PirataDto>>(piratas);

            resultado.CurrentPage = piratas.CurrentPage; // mapeamento manual de propriedades
            resultado.TotalPages = piratas.TotalPages;
            resultado.PageSize = piratas.PageSize;
            resultado.TotalCount = piratas.TotalCount;

            return resultado;
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    public async Task<PirataDto> GetPirataByIdAsync(int pirataId)
    {
        try
        {
            var pirata = await _pirataPersist.GetPirataByIdAsync(pirataId);
            if (pirata == null) return null;
            var resultado = _mapper.Map<PirataDto>(pirata);
            return resultado;
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    public async Task<PirataDto> UpdatePirata(int pirataId, PirataDto model)
    {
        try
        {
            var pirata = await _pirataPersist.GetPirataByIdAsync(pirataId);
            if (pirata == null) return null;
            model.Id = pirata.Id;
            _mapper.Map(model, pirata); //atualizando o pirata com os dados do model, mapeia o model para o pirata
            _geralPersist.Update<PirataModel>(pirata);
            if (await _geralPersist.SaveChangesAsync())
            {
                var pirataRetorno = await _pirataPersist.GetPirataByIdAsync(pirata.Id);
                return _mapper.Map<PirataDto>(pirataRetorno); //mapeando para o tipo de retorno
            }
            return null;
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }
    private async Task<CapitaoDto> GetCapitaoByIdOrNome(CapitaoDto capitao)
    {
        if (capitao.Id > 0)
        {
            var capitaoExistenteId = await _capitaoPersist.GetCapitaoByIdAsync(capitao.Id);
            if (capitaoExistenteId != null)
            {
                _geralPersist.Detach<CapitaoModel>(capitaoExistenteId); //desacoplando o capitaoExistenteId
                return _mapper.Map<CapitaoDto>(capitaoExistenteId);
            }
        }
        else if (capitao.Nome != null)
        {
            var capitaoExistenteNome = await _capitaoPersist.GetCapitaoByNomeAsync(capitao.Nome);
            if (capitaoExistenteNome != null)
            {
                _geralPersist.Detach<CapitaoModel>(capitaoExistenteNome); //desacoplando o capitaoExistenteNome
                return _mapper.Map<CapitaoDto>(capitaoExistenteNome);
            }
        }
        return capitao;
    }

    private async Task<CapitaoDto> GetCapitaoId(PirataDto pirataDto, CapitaoDto capitao)
    {
        var capitaoIdExistente = await _capitaoPersist.GetCapitaoByIdAsync(pirataDto.CapitaoId.Value);
        if (capitaoIdExistente != null)
        {
            _geralPersist.Detach<CapitaoModel>(capitaoIdExistente); //desacoplando o capitaoIdExistente
            return _mapper.Map<CapitaoDto>(capitaoIdExistente);
        }
        return capitao;
    }


}