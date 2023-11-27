using BinksNoSake.API.Extensions;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BinksNoSake.API.Controllers;
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ITokenService _tokenService;
    public AccountController(IAccountService accountService, ITokenService tokenService)
    {
        _tokenService = tokenService;
        _accountService = accountService;
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

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AccountLoginDto accountLoginDto)
    {
        try
        {
            var user = await _accountService.GetUserByUsernameAsync(accountLoginDto.Username);
            if (user == null) return Unauthorized("Usuário não cadastrado!");

            var result = await _accountService.CheckUserPasswordAsync(user, accountLoginDto.Password);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    username = user.Username,
                    primeiroNome = user.Username,
                    token = await _tokenService.CreateToken(user)
                });
            }
            return Unauthorized("Usuário ou senha incorretos!");
        }
        catch (System.Exception e)
        {

            throw new Exception($"Erro ao tentar logar! {e.Message}");
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromBody] AccountUpdateDto accountUpdateDto)
    {
        try
        {
            if (accountUpdateDto.Username != User.GetUserName()) return Unauthorized("Usuário Inválido");

            var user = await _accountService.GetUserByUsernameAsync(User.GetUserName());
            if (user == null) return Unauthorized("Usuário Inválido");

            var userReturn = await _accountService.UpdateAccount(accountUpdateDto);
            if (userReturn == null) return NoContent();

            return Ok(userReturn);
        }
        catch (Exception e)
        {
            throw new Exception($"Erro ao tentar atualizar usuário: {e.Message}");
        }
    }

    [HttpGet("GetUser", Name = "GetUser")]
    public async Task<IActionResult> GetUser()
    {
        try
        {
            var username = User.GetUserName();
            var user = await _accountService.GetUserByUsernameAsync(username);
            return Ok(user);
        }
        catch (System.Exception e)
        {
            throw new Exception($"Erro ao tentar obter usuário! {e.Message}");
        }
    }
}