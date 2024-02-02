using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BinksNoSake.API.Extensions;
using BinksNoSake.API.Helpers;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using BinksNoSake.Domain.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BinksNoSake.API.Controllers;
[ApiController]
[Route("[controller]")]
public class AcessoController : ControllerBase
{
    private static List<Account> UserList = new List<Account>();
    private readonly IAccountService _accountService;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<Account> _signInManager;
    private readonly UserManager<Account> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IUtil _util;
    public AcessoController(IAccountService accountService,
                            ITokenService tokenService,
                            SignInManager<Account> signInManager,
                            UserManager<Account> userManager,
                            IConfiguration configuration,
                            IMapper mapper,
                            IUtil util)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
        _accountService = accountService;
        _configuration = configuration;
        _mapper = mapper;
        _util = util;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AccountLoginDto accountLoginDto)
    {
        try
        {
            var user = await _accountService.GetUserByCredentialAsync(accountLoginDto.Username);
            if (user == null) return NotFound("O email ou username que você informou não está conectado a uma conta.");

            var result = await _accountService.CheckUserPasswordAsync(user, accountLoginDto.Password);
            if (result.Succeeded)
            {
                user.Token = await _tokenService.CreateToken(user);
                user.RefreshToken = await _tokenService.GenereteRefreshToken();
                _tokenService.SaveRefreshToken(user.Username, user.RefreshToken);
                return Ok(new
                {
                    username = user.Username,
                    nome = user.Nome,
                    sobrenome = user.Sobrenome,
                    email = user.Email,
                    imagemURL = user.ImagemURL,
                    funcao = user.Funcao,
                    token = user.Token,
                    refreshToken = user.RefreshToken,
                });
            }
            return Unauthorized("Usuário e/ou Senha incorreto(s)");
        }
        catch (System.Exception e)
        {

            throw new Exception($"Erro ao tentar logar! {e.Message}");
        }
    }

    [HttpPost("LoginWithGoogle")]
    [AllowAnonymous]
    public async Task<IActionResult> ExchangeFirebaseToken([FromBody] FirebaseTokenRequest request)
    {
        try
        {
            var firebaseToken = request.FireBaseToken;
            var validatedToken = await ValidateFirebaseToken(firebaseToken);

            var existingUser = await _accountService.GetUserByEmailAsync(validatedToken.Email);

            if (existingUser == null)
            {
                var generatedUsername = GeneratedValidUsername(validatedToken.Name);
                var uniqueUsername = await GenerateUniqueUsername(generatedUsername);

                var nameParts = validatedToken.Name.Split(" ");
                var nome = nameParts.Length > 0 ? nameParts[0] : null;
                var sobrenome = nameParts.Length > 1 ? nameParts[1] : null;

                var accountUpdateDto = new AccountUpdateDto
                {
                    Username = uniqueUsername,
                    Email = validatedToken.Email,
                    Nome = validatedToken.GivenName ?? nome ?? null,
                    Sobrenome = validatedToken.Family_name ?? sobrenome ?? null,
                    ImagemURL = validatedToken.Picture ?? null,
                };

                accountUpdateDto.Email = validatedToken.Email;

                var userMapped = _mapper.Map<Account>(accountUpdateDto);
                var result = await _userManager.CreateAsync(userMapped);
                if (result.Succeeded)
                {
                    var userResult = await _accountService.GetUserByUsernameAsync(uniqueUsername);
                    if (userResult != null)
                    {
                        existingUser = userResult;
                    }
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }

            existingUser.Token = await _tokenService.CreateToken(existingUser);
            existingUser.RefreshToken = await _tokenService.GenereteRefreshToken();
            _tokenService.SaveRefreshToken(existingUser.Username, existingUser.RefreshToken);

            return Ok(new
            {
                username = existingUser.Username,
                nome = existingUser.Nome,
                sobrenome = existingUser.Sobrenome,
                email = existingUser.Email,
                imagemURL = existingUser?.ImagemURL,
                funcao = existingUser.Funcao,
                token = existingUser.Token,
                refreshToken = existingUser.RefreshToken,
            });
        }
        catch (System.Exception e)
        {
            throw new Exception($"Erro ao tentar logar! {e.Message}");
        }
    }

    [NonAction]
    private async Task<FirebaseToken> ValidateFirebaseToken(string fireBaseToken)
    {
        try
        {

            var parts = fireBaseToken.Split('.');

            if (parts.Length != 3)
            {
                throw new Exception("Token inválido");
            }

            var payload = parts[1];

            var decodedPayloadBytes = Base64UrlEncoder.DecodeBytes(payload);
            var payloadJson = Encoding.UTF8.GetString(decodedPayloadBytes);

            var firebaseValidatedToken = JsonConvert.DeserializeObject<FirebaseToken>(payloadJson);

            return firebaseValidatedToken;
        }
        catch (System.Exception e)
        {
            throw new Exception($"Erro ao validar token do firebase: {e.Message}");
        }
    }


    private string GeneratedValidUsername([FromBody] string username)
    {
        string validUsername = new string(username.Where(c => char.IsLetterOrDigit(c)).ToArray());

        return validUsername;
    }

    private async Task<string> GenerateUniqueUsername(string baseUsername)
    {
        string generatedUsername = baseUsername;
        generatedUsername = await _accountService.RemoveAccents(generatedUsername);

        while (await _accountService.UserExists(generatedUsername))
        {
            generatedUsername = baseUsername + Guid.NewGuid().ToString().Substring(0, 4);
        }

        return generatedUsername;
    }

    [HttpPost("refreshToken")]
    [Authorize]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            var username = principal.Identity.Name;
            var savedRefreshToken = await _tokenService.GetRefreshToken(username);

            if (savedRefreshToken != null)
            {
                _tokenService.DeleteRefreshToken(username, savedRefreshToken);
            }

            var newJwtToken = await _tokenService.CreateTokenEnumerator(principal.Claims);
            var newRefreshToken = await _tokenService.GenereteRefreshToken();

            var refreshTokenExpiration = DateTime.UtcNow.Add(_tokenService.RefreshTokenExpiration);

            await _tokenService.DeleteExpiredRefreshToken();

            return Ok(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status401Unauthorized, $"Erro {e.Message}");
        }
    }
}