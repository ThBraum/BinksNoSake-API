using BinksNoSake.API.Extensions;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BinksNoSake.API.Controllers;
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ITokenService _tokenService;
    public AccountController(IAccountService accountService, ITokenService tokenService)
    {
        _tokenService = tokenService;
        _accountService = accountService;
    }

    [HttpGet("GetUser", Name = "GetUser")]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        try
        {
            var username = User.GetUserName();
            var user = await _accountService.GetUserByUsernameAsync(username);
            user.RefreshToken = await _tokenService.GetRefreshToken(user.Username);
            return Ok(user);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }


    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] AccountDto accountDto)
    {
        try
        {
            if (await _accountService.UserExists(accountDto.Username)) return BadRequest("Username já cadastrado!");

            var user = await _accountService.CreateAccountAsync(accountDto);
            if (user != null)
            {
                var userToReturn = _accountService.GetUserByUsernameAsync(accountDto.Username);
                return Created("", userToReturn);
            }
            return BadRequest("Erro ao tentar adicionar usuário!");
        }
        catch (System.Exception e)
        {

            throw new Exception($"Erro ao tentar adicionar usuário! {e.Message}");
        }
    }

    [HttpPut("update")]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromBody] AccountUpdateDto accountUpdateDto)
    {
        try
        {
            if (accountUpdateDto.Username != User.GetUserName()) return Unauthorized("Usuário Inválido");

            var user = await _accountService.GetUserByUsernameAsync(User.GetUserName());
            if (user == null) return Unauthorized("Usuário Inválido");

            if (accountUpdateDto.ImagemURL != null)
            {
                await uploadAccountImage(accountUpdateDto.ImagemURL);
            }

            var userReturn = await _accountService.UpdateAccount(accountUpdateDto);
            if (userReturn == null) return NoContent();

            return Ok(userReturn);
        }
        catch (Exception e)
        {
            throw new Exception($"Erro ao tentar atualizar usuário: {e.Message}");
        }
    }

    // [HttpPost("upload-image", Name = "UploadImage")]
    // public async Task<IActionResult> UploadImage()
    // {
    //     try
    //     {
    //         var user = await _accountService.GetUserByUsernameAsync(User.GetUserName());
    //         if (user == null) return NoContent();

    //         var file = Request.Form.Files[0];
    //         if (file.Length > 0)
    //         {
    //             _accountService.DeleteImage(User.GetUserId(), user.ImagemURL);
    //             user.ImagemURL = await _accountService.SaveImage(file);
    //         }

    //         var eventoRetorno = await _accountService.UpdateAccount(user);
    //         if (eventoRetorno == null) return BadRequest("Erro ao tentar atualizar evento");
    //         return Ok(eventoRetorno);
    //     }
    //     catch (System.Exception e)
    //     {
    //         return this.StatusCode(StatusCodes.Status500InternalServerError, 
    //             $"Erro ao realizar upload de imagem. Erro: {e.Message}");
    //     }
    // }


    [NonAction]
    public async Task<string> uploadAccountImage(string imageName)
    {
        try
        {
            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                _accountService.DeleteImage(User.GetUserId(), imageName);
                imageName = await _accountService.SaveImage(file);
            }
            return imageName;
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}