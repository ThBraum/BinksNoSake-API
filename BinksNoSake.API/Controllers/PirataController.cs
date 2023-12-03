using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BinksNoSake.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PirataController : ControllerBase
{
    private readonly IPirataService _pirataService;
    public PirataController(IPirataService pirataService)
    {
        _pirataService = pirataService;

    }

    [HttpGet(Name = "GetAllPiratas")]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
        try
        {
            var piratas = await _pirataService.GetAllPiratasAsync();
            if (piratas == null) return NoContent();
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

    [HttpGet("nome/{nome}", Name = "GetPirataByNome")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(string nome)
    {
        try
        {
            var piratas = await _pirataService.GetAllPiratasByNomeAsync(nome);
            if (piratas == null) return NoContent();
            return Ok(piratas);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, 
            $"Erro ao tentar recuperar piratas. Erro: {e.Message}");
        }
    }

    [HttpPost(Name = "AddPirata")]
    [Authorize]
    public async Task<IActionResult> Post(PirataDto model)
    {
        try
        {   
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