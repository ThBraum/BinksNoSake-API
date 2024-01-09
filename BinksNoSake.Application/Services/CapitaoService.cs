using AutoMapper;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;
using BinksNoSake.Persistence.Pagination;
using Microsoft.EntityFrameworkCore;

namespace BinksNoSake.Application.Services;
public class CapitaoService : ICapitaoService
{
    private readonly IGeralPersist _geralPersist;
    private readonly ICapitaoPersist _capitaoPersist;
    private readonly IMapper _mapper;
    private readonly IPirataPersist _pirataPersist;

    public CapitaoService(IGeralPersist geralPersist, ICapitaoPersist capitaoPersist, IMapper mapper, IPirataPersist pirataPersist)
    {
        _mapper = mapper;
        _capitaoPersist = capitaoPersist;
        _geralPersist = geralPersist;
        _pirataPersist = pirataPersist;
    }

    public async Task<CapitaoDto> AddCapitao(CapitaoDto model)
    {
        try
        {
            var capitao = _mapper.Map<CapitaoModel>(model);

            _geralPersist.Detach(capitao);
            foreach (var pirata in capitao.Piratas)
            {
                _geralPersist.Detach(pirata);
            }
            capitao.Piratas.Clear();
            await _capitaoPersist.AddCapitaoWithExistingPiratasAsync(capitao, model.Piratas.Select(p => p.Id).ToList());
            if (await _geralPersist.SaveChangesAsync())
            {
                capitao = await _capitaoPersist.GetCapitaoByIdAsync(capitao.Id);
                return _mapper.Map<CapitaoDto>(capitao);
            }
            return null;
        }
        catch (Exception e)
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

    public async Task<PageList<CapitaoDto>> GetAllCapitaesAsync(PageParams pageParams)
    {
        try
        {
            var capitaes = await _capitaoPersist.GetAllCapitaesAsync(pageParams);
            if (capitaes == null) return null;
            var resultado = _mapper.Map<PageList<CapitaoDto>>(capitaes);

            resultado.CurrentPage = capitaes.CurrentPage; // mapeamento manual de propriedades
            resultado.TotalPages = capitaes.TotalPages;
            resultado.PageSize = capitaes.PageSize;
            resultado.TotalCount = capitaes.TotalCount;

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