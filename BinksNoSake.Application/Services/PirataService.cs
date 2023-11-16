using AutoMapper;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;

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
            var capitaoExistente = await _capitaoPersist.GetAllCapitaesByNomeAsync(model.Capitao.Nome);
            var capitaoAssociado = capitaoExistente.FirstOrDefault();

            //Se não existir um capitão com o mesmo nome e está sendo passado um capitão junto com o pirata
            if (capitaoAssociado == null && model.Capitao != null)
            {
                capitaoAssociado = _mapper.Map<CapitaoModel>(model.Capitao);
                _geralPersist.Add<CapitaoModel>(capitaoAssociado);
            }

            model.Capitao = capitaoAssociado;

            var pirata = _mapper.Map<PirataModel>(model);
            _geralPersist.Add<PirataModel>(pirata);

            if (await _geralPersist.SaveChangesAsync())
            {
                var pirataRetorno = await _pirataPersist.GetPirataByIdAsync(pirata.PirataId);
                return _mapper.Map<PirataDto>(pirataRetorno);
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

    public async Task<PirataDto[]> GetAllPiratasAsync()
    {
        try
        {
            var piratas = await _pirataPersist.GetAllPiratasAsync();
            if (piratas == null) return null;
            var resultado = _mapper.Map<PirataDto[]>(piratas); //mapeando para o tipo de retorno
            return resultado;
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    public async Task<PirataDto[]> GetAllPiratasByNomeAsync(string nome)
    {
        var piratas = await _pirataPersist.GetAllPiratasByNomeAsync(nome);
        if (piratas == null) return null;
        var resultado = _mapper.Map<PirataDto[]>(piratas);
        return resultado;
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
            if (pirata == null) throw new Exception("Pirata não encontrado.");
            model.PirataId = pirata.PirataId;
            _mapper.Map(model, pirata); //atualizando o pirata com os dados do model, mapeia o model para o pirata
            _geralPersist.Update<PirataModel>(pirata);
            if (await _geralPersist.SaveChangesAsync())
            {
                var pirataRetorno = await _pirataPersist.GetPirataByIdAsync(pirata.PirataId);
                return _mapper.Map<PirataDto>(pirataRetorno); //mapeando para o tipo de retorno
            }
            return null;
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }
}