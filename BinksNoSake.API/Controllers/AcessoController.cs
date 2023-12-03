using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BinksNoSake.API.Controllers;
[ApiController]
[Route("[controller]")]
public class AcessoController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ITokenService _tokenService;
    public AcessoController(IAccountService accountService, ITokenService tokenService)
    {
        _tokenService = tokenService;
        _accountService = accountService;
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
                user.Token = await _tokenService.CreateToken(user);
                user.RefreshToken = await _tokenService.GenereteRefreshToken();
                _tokenService.SaveRefreshToken(user.Username, user.RefreshToken);
                return Ok(new
                {
                    username = user.Username,
                    primeiroNome = user.PrimeiroNome,
                    token = user.Token,
                    refreshToken = user.RefreshToken
                });
            }
            return Unauthorized("Usuário ou senha incorretos!");
        }
        catch (System.Exception e)
        {

            throw new Exception($"Erro ao tentar logar! {e.Message}");
        }
    }

    [HttpPost("refreshToken")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken(string token, string refreshToken)
    {
        try
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            var username = principal.Identity.Name;
            var savedRefreshToken = await _tokenService.GetRefreshToken(username);
            if (savedRefreshToken != refreshToken) return BadRequest("Refresh Token inválido!");

            var newJwtToken = await _tokenService.CreateTokenEnumerator(principal.Claims);
            var newRefreshToken = await _tokenService.GenereteRefreshToken();

            _tokenService.DeleteRefreshToken(username, refreshToken);
            _tokenService.SaveRefreshToken(username, newRefreshToken);
            return Ok(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }
        catch (System.Exception e)
        {

            throw new Exception($"Erro ao tentar logar! {e.Message}");
        }
    }
}