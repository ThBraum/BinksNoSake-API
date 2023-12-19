using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BinksNoSake.API.Extensions;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using BinksNoSake.Domain.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
    public AcessoController(IAccountService accountService, ITokenService tokenService, SignInManager<Account> signInManager, UserManager<Account> userManager, IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
        _accountService = accountService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AccountLoginDto accountLoginDto)
    {
        try
        {
            var user = await _accountService.GetUserByUsernameAsync(accountLoginDto.Username);
            if (user == null) return NotFound("Usuário não cadastrado");

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
            return Unauthorized("Usuário e/ou Senha incorreto(s)");
        }
        catch (System.Exception e)
        {

            throw new Exception($"Erro ao tentar logar! {e.Message}");
        }
    }

    [HttpPost("LoginWithGoogle")]
    public async Task<IActionResult> LoginWithGoogleExternal([FromBody] string credential)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _configuration["Google:ClientId"] }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

            var generatedUsername = GeneratedValidUsername(payload.Name);
            
            var user = await _accountService.GetUserByUsernameAsync(generatedUsername);

            if (user == null)
            {
                var newUser = new Account
                {
                    UserName = generatedUsername,
                    Email = payload.Email,
                    PrimeiroNome = payload.GivenName,
                    UltimoNome = payload.FamilyName,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(newUser);
                if (!result.Succeeded) return BadRequest(result.Errors);

                user = await _accountService.GetUserByUsernameAsync(payload.Name);
            }

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
        catch (System.Exception e)
        {
            throw new Exception($"Erro ao tentar logar! {e.Message}");
        }
    }

    private string GeneratedValidUsername(string username)
    {
        string validUsername = new string(username.Where(c =>  char.IsLetterOrDigit(c)).ToArray());

        return validUsername;
    }

    [NonAction]
    public dynamic JWTGenerator(Account user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
        };
        
        var roles = _userManager.GetRolesAsync(user);
        foreach (var role in roles.Result)
        {
            new Claim(ClaimTypes.Role, role);
        }

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = creds
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new { token = tokenString, username = user.UserName };
    }

    [HttpPost("refreshToken")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            var username = principal.Identity.Name;
            var savedRefreshToken = await _tokenService.GetRefreshToken(username);

            var newJwtToken = await _tokenService.CreateTokenEnumerator(principal.Claims);
            var newRefreshToken = await _tokenService.GenereteRefreshToken();

            _tokenService.DeleteRefreshToken(username, savedRefreshToken);
            _tokenService.SaveRefreshToken(username, newRefreshToken);
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

internal interface IMapper
{
}