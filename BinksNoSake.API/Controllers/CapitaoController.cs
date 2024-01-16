using BinksNoSake.API.Extensions;
using BinksNoSake.API.Helpers;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using BinksNoSake.Persistence.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BinksNoSake.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CapitaoController : ControllerBase
{
    private readonly ICapitaoService _capitaoService;
    private readonly IUtil _uti;

    public CapitaoController(ICapitaoService capitaoService, IUtil util)
    {
        _capitaoService = capitaoService;
        _uti = util;
    }

    [HttpGet(Name = "GetAllCapitaes")]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
    {
        try
        {
            var capitaes = await _capitaoService.GetAllCapitaesAsync(pageParams);
            if (capitaes == null) return NoContent();
            
            Response.AddPagination(capitaes.CurrentPage, capitaes.PageSize, capitaes.TotalCount, capitaes.TotalPages);
            
            return Ok(capitaes);
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    [HttpGet("{id}", Name = "GetCapitaoById")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var capitao = await _capitaoService.GetCapitaoByIdAsync(id);
            if (capitao == null) return NoContent();
            return Ok(capitao);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    [HttpPost(Name = "AddCapitao")]
    [Authorize]
    public async Task<IActionResult> Post([FromForm] CapitaoDto model)
    {
        try
        {
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                model.ImagemURL = await _uti.SaveImage(file);
            }

            var capitao = await _capitaoService.AddCapitao(model);
            if (capitao == null) return BadRequest("Erro ao tentar adicionar capitao.");
            return Ok(capitao);
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    [HttpPut("{id}", Name = "UpdateCapitao")]
    [Authorize]
    public async Task<IActionResult> Put(int id, CapitaoDto model)
    {
        try
        {
            var capitao = await _capitaoService.UpdateCapitao(id, model);
            if (capitao == null) return BadRequest("Erro ao tentar atualizar capitao.");
            return Ok(capitao);
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    [HttpDelete("{id}", Name = "DeleteCapitao")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var capitao = await _capitaoService.DeleteCapitao(id);
            if (capitao == false) return BadRequest("Erro ao tentar deletar capitao.");
            return Ok("Deletado com sucesso!");
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }
}