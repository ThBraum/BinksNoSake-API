using AutoMapper;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;

namespace BinksNoSake.Application.Services;
public class CapitaoService : ICapitaoService
{
    private readonly IGeralPersist _geralPersist;
    private readonly ICapitaoPersist _capitaoPersist;
    private readonly IMapper _mapper;

    public CapitaoService(IGeralPersist geralPersist, ICapitaoPersist capitaoPersist, IMapper mapper)
    {
        _mapper = mapper;
        _capitaoPersist = capitaoPersist;
        _geralPersist = geralPersist;
    }

    public async Task<CapitaoDto> AddCapitao(CapitaoDto model)
    {
        try
        {
            var capitao = _mapper.Map<CapitaoModel>(model);
            _geralPersist.Add<CapitaoModel>(capitao);
            if (await _geralPersist.SaveChangesAsync())
            {
                var capitaoRetorno = await _capitaoPersist.GetCapitaoByIdAsync(capitao.Id);
                return _mapper.Map<CapitaoDto>(capitaoRetorno);
            }
            return null;
        }
        catch (System.Exception e)
        {
            
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> DeleteCapitao(int capitaoId)
    {
        try
        {
            var capitao = await _capitaoPersist.GetCapitaoByIdAsync(capitaoId);
            if (capitao == null) throw new Exception("Capitão não encontrado.");
            _geralPersist.Delete<CapitaoModel>(capitao);
            if (await _geralPersist.SaveChangesAsync()) return true;
            return false;
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    public async Task<CapitaoDto[]> GetAllCapitaesAsync()
    {
        try
        {
            var capitao = await _capitaoPersist.GetAllCapitaesAsync();
            if (capitao == null) return null;
            var resultado = _mapper.Map<CapitaoDto[]>(capitao);
            return resultado;
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    public async Task<CapitaoDto> GetCapitaoByNomeAsync(string nome)
    {
        try
        {
            var capitao = await _capitaoPersist.GetCapitaoByNomeAsync(nome);
            if (capitao == null) return null;
            var resultado = _mapper.Map<CapitaoDto>(capitao);
            return resultado;
        }
        catch (System.Exception e)
        {
            
            throw new Exception(e.Message);
        }
    }

    public async Task<CapitaoDto> GetCapitaoByIdAsync(int capitaoId)
    {
        try
        {
            var capitao = await _capitaoPersist.GetCapitaoByIdAsync(capitaoId);
            if (capitao == null) return null;
            var resultado = _mapper.Map<CapitaoDto>(capitao);
            return resultado;
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    public async Task<CapitaoDto> UpdateCapitao(int capitaoId, CapitaoDto model)
    {
        try
        {
            var capitao = await _capitaoPersist.GetCapitaoByIdAsync(capitaoId);
            if (capitao == null) throw new Exception("Capitão não encontrado.");
            model.Id = capitao.Id;
            _mapper.Map(model, capitao);
            _geralPersist.Update<CapitaoModel>(capitao);
            if (await _geralPersist.SaveChangesAsync())
            {
                var capitaoRetorno = await _capitaoPersist.GetCapitaoByIdAsync(capitao.Id);
                return _mapper.Map<CapitaoDto>(capitaoRetorno);
            }
            return null;
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }
}