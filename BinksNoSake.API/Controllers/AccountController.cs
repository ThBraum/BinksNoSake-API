using System.IdentityModel.Tokens.Jwt;
using BinksNoSake.API.Extensions;
using BinksNoSake.API.Helpers;
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
    private readonly IUtil _util;

    private readonly string _destino = "Images";
    public AccountController(IAccountService accountService, ITokenService tokenService, IUtil util)
    {
        _tokenService = tokenService;
        _accountService = accountService;
        _util = util;
    }

    [HttpGet("GetUser", Name = "GetUser")]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        try
        {
            var username = User.GetUserName();
            var user = await _accountService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return NoContent();
            }

            var jwtToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(jwtToken) as JwtSecurityToken;

            if (jsonToken != null && jsonToken.ValidTo < DateTime.UtcNow)
            {
                user.Token = await _tokenService.CreateToken(user);
                await _accountService.UpdateAccount(user);
                user.RefreshToken = await _tokenService.GetRefreshToken(user.Username);
            }

            return Ok(new
            {
                id = user.Id,
                username = user.Username,
                nome = user.Nome,
                sobrenome = user.Sobrenome,
                email = user.Email,
                imagemURL = user.ImagemURL,
                funcao = user.Funcao,
                phoneNumber = user.PhoneNumber,
                refreshToken = user.RefreshToken
            });
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao obter usuário: {e.Message}");
        }
    }


    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromForm] AccountDto accountDto)
    {
        try
        {
            if (await _accountService.UserExists(accountDto.Username)) return Conflict("Username já cadastrado!");
            if (await _accountService.GetUserByEmailAsync(accountDto.Email) != null) return Conflict("Email já cadastrado!");

            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                accountDto.ImagemURL = await _util.SaveImage(file, _destino);
            }
            
            var user = await _accountService.CreateAccountAsync(accountDto);
            if (user != null)
            {
                var userToReturn = _accountService.GetUserByUsernameAsync(accountDto.Username);
                var token = await _tokenService.CreateToken(userToReturn.Result);

                var response = new
                {
                    user = userToReturn,
                    Token = token
                };
                return Created("", response);
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
    public async Task<IActionResult> UpdateUser([FromForm] AccountUpdateDto accountUpdateDto)
    {
        try
        {
            if (accountUpdateDto.Username != User.GetUserName())
            {
                var IsUsernamemAvailable = await _accountService.IsUsernameAvailable(accountUpdateDto.Username);
                if (!IsUsernamemAvailable) return Conflict("Username não disponível. Tente outro.");
            }

            var user = await _accountService.GetUserByUsernameAsync(User.GetUserName());
            if (user == null) return Unauthorized("Usuário Inválido");

            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                _util.DeleteImage(user.ImagemURL, _destino);
                accountUpdateDto.ImagemURL = await _util.SaveImage(file, _destino);
            } 
            else
            {
                accountUpdateDto.ImagemURL = user.ImagemURL;
            }

            // Percorre todas as propriedades do DTO para verificar se alguma está nula ou vazia. Se estiver, atribui o valor da propriedade do usuário atual.
            foreach (var prop in accountUpdateDto.GetType().GetProperties())
            {
                var currentUserValue = prop.GetValue(user);
                var dtoValue = prop.GetValue(accountUpdateDto);

                if (dtoValue == null || string.IsNullOrEmpty(dtoValue.ToString()))
                {
                    prop.SetValue(accountUpdateDto, currentUserValue);
                }
            }

            var userReturn = await _accountService.UpdateAccount(accountUpdateDto, user);
            if (userReturn == null) return NoContent();

            return Ok(userReturn);
        }
        catch (Exception e)
        {
            throw new Exception($"Erro ao tentar atualizar usuário: {e.Message}");
        }
    }
}