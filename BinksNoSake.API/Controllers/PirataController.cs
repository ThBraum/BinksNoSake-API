using System.Globalization;
using BinksNoSake.API.Extensions;
using BinksNoSake.API.Helpers;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using BinksNoSake.Persistence.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BinksNoSake.API.Controllers;

/// <summary>
/// Controlador responsável pela gestão de piratas.
/// </summary>
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

    /// <summary>
    /// Retorna uma lista paginada de piratas.
    /// </summary>
    /// <param name="pageParams">Parâmetros de paginação para controlar o tamanho da página e o número da página.</param>
    /// <returns>Uma lista paginada de piratas.</returns>
    /// <response code="200">Retorna a lista paginada de piratas.</response>
    /// <response code="204">Retorna se não houver piratas.</response>
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

    /// <summary>
    /// Retorna os detalhes de um pirata específico pelo ID.
    /// </summary>
    /// <param name="id">O ID do pirata a ser recuperado.</param>
    /// <returns>Detalhes do pirata solicitado.</returns>
    /// <response code="200">Retorna os detalhes do pirata.</response>
    /// <response code="204">Retorna se o pirata não for encontrado.</response>
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


    /// <summary>
    /// Adiciona um novo pirata ao sistema.
    /// Requer autenticação.
    /// </summary>
    /// <param name="model">Dados do pirata para adicionar.</param>
    /// <returns>Os detalhes do pirata adicionado.</returns>
    /// <response code="200">Retorna os detalhes do pirata adicionado.</response>
    /// <response code="400">Retorna se houver um erro ao adicionar o pirata.</response>
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

    /// <summary>
    /// Atualiza os detalhes de um pirata existente.
    /// Requer autenticação.
    /// </summary>
    /// <param name="id">O ID do pirata a ser atualizado.</param>
    /// <param name="model">Os novos dados do pirata.</param>
    /// <returns>Os detalhes do pirata atualizado.</returns>
    /// <response code="200">Retorna os detalhes do pirata atualizado.</response>
    /// <response code="400">Retorna se houver um erro ao atualizar o pirata.</response>
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

    /// <summary>
    /// Deleta um pirata do sistema.
    /// Requer autenticação.
    /// </summary>
    /// <param name="id">O ID do pirata a ser deletado.</param>
    /// <returns>Uma mensagem indicando o sucesso da operação.</returns>
    /// <response code="200">Retorna se o pirata foi deletado com sucesso.</response>
    /// <response code="400">Retorna se não foi possível deletar o pirata.</response>
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