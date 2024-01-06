using System.Globalization;
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
public class PirataController : ControllerBase
{
    private readonly IPirataService _pirataService;
    private readonly IUtil _util;

    private readonly string _destino = "Images";
    public PirataController(IPirataService pirataService, IUtil util)
    {
        _pirataService = pirataService;
        _util = util;
    }

    [HttpGet(Name = "GetAllPiratas")]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
    {
        try
        {
            var piratas = await _pirataService.GetAllPiratasAsync(pageParams);
            if (piratas == null) return NoContent();

            Response.AddPagination(piratas.CurrentPage, piratas.PageSize, piratas.TotalCount, piratas.TotalPages);

            return Ok(piratas);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar recuperar piratas. Erro: {e.Message}");
        }
    }

    [HttpGet("{id}", Name = "GetPirataById")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var pirata = await _pirataService.GetPirataByIdAsync(id);
            if (pirata == null) return NoContent();
            return Ok(pirata);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar recuperar pirata. Erro: {e.Message}");
        }
    }


    [HttpPost(Name = "AddPirata")]
    [Authorize]
    public async Task<IActionResult> Post([FromForm] PirataDto model)
    {
        try
        {
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                model.ImagemURL = await _util.SaveImage(file, _destino);
            }

            var pirata = await _pirataService.AddPirata(model);
            if (pirata == null) return BadRequest("Erro ao tentar adicionar pirata.");
            return Ok(pirata);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar adicionar pirata. Erro: {e.Message}");
        }
    }

    [HttpPut("{id}", Name = "UpdatePirata")]
    [Authorize]
    public async Task<IActionResult> Put(int id, PirataDto model)
    {
        try
        {
            var pirata = await _pirataService.UpdatePirata(id, model);
            if (pirata == null) return BadRequest("Erro ao tentar atualizar pirata.");
            return Ok(pirata);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar atualizar pirata. Erro: {e.Message}");
        }
    }

    [HttpDelete("{id}", Name = "DeletePirata")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            return await _pirataService.DeletePirata(id) ?
            Ok("Deletado") :
            BadRequest("Pirata não deletado");
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar deletar pirata. Erro: {e.Message}");
        }
    }
}