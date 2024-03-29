using BinksNoSake.API.Extensions;
using BinksNoSake.API.Helpers;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using BinksNoSake.Persistence.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BinksNoSake.API.Controllers;

/// <summary>
/// Controlador responsável pelo gerenciamento de capitães.
/// </summary>
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

    /// <summary>
    /// Retorna uma lista paginada de capitães.
    /// </summary>
    /// <param name="pageParams">Parâmetros de paginação.</param>
    /// <returns>Uma lista paginada de capitães.</returns>
    /// <response code="200">Retorna a lista paginada de capitães.</response>
    /// <response code="204">Retorna se não houver capitães.</response>
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

    /// <summary>
    /// Retorna os detalhes de um capitão específico pelo ID.
    /// </summary>
    /// <param name="id">O ID do capitão.</param>
    /// <returns>Detalhes do capitão.</returns>
    /// <response code="200">Retorna os detalhes do capitão.</response>
    /// <response code="204">Retorna se o capitão não for encontrado.</response>
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

    /// <summary>
    /// Adiciona um novo capitão ao sistema.
    /// Requer autenticação.
    /// </summary>
    /// <param name="model">Dados do capitão para adicionar.</param>
    /// <returns>Os detalhes do capitão adicionado.</returns>
    /// <response code="200">Retorna os detalhes do capitão adicionado.</response>
    /// <response code="400">Retorna se houver um erro ao adicionar o capitão.</response>
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

    /// <summary>
    /// Atualiza os detalhes de um capitão existente.
    /// Requer autenticação.
    /// </summary>
    /// <param name="id">O ID do capitão a ser atualizado.</param>
    /// <param name="model">Os novos dados do capitão.</param>
    /// <returns>Os detalhes do capitão atualizado.</returns>
    /// <response code="200">Retorna os detalhes do capitão atualizado.</response>
    /// <response code="400">Retorna se houver um erro ao atualizar o capitão.</response>
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

    /// <summary>
    /// Deleta um capitão do sistema.
    /// Requer autenticação.
    /// </summary>
    /// <param name="id">O ID do capitão a ser deletado.</param>
    /// <returns>Uma mensagem indicando sucesso.</returns>
    /// <response code="200">Retorna se o capitão foi deletado com sucesso.</response>
    /// <response code="400">Retorna se houver um erro ao tentar deletar o capitão.</response>
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