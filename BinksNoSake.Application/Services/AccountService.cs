using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using BinksNoSake.Domain.Identity;
using BinksNoSake.Persistence.Contratos;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BinksNoSake.Application.Services;
public class AccountService : IAccountService
{
    private readonly UserManager<Account> _userManager;
    private readonly SignInManager<Account> _signInManager;
    private readonly IAccountPersist _accountPersist;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IConfigurationSection _googleAuthSettings;

    public AccountService(UserManager<Account> userManager,
                        SignInManager<Account> signInManager,
                        IMapper mapper,
                        IAccountPersist accountPersist,
                        IWebHostEnvironment hostEnvironment,
                        IConfiguration configuration)
    {
        _hostEnvironment = hostEnvironment;
        _accountPersist = accountPersist;
        _signInManager = signInManager;
        _userManager = userManager;
        _userManager.KeyNormalizer = new UpperInvariantLookupNormalizer();
        _mapper = mapper;
        _googleAuthSettings = configuration.GetSection("GoogleAuthSettings");
    }

    public async Task<SignInResult> CheckUserPasswordAsync(AccountUpdateDto accountUpdateDto, string password)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(accountUpdateDto.Username.ToLower());

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(accountUpdateDto.Email.ToLower());
                if (user == null) return SignInResult.Failed;
            }
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }
        catch (System.Exception e)
        {

            throw new Exception($"Erro ao verificar senha. {e.Message}");
        }
    }

    public async Task<AccountDto> CreateAccountAsync(AccountDto accountDto)
    {
        try
        {
            var user = _mapper.Map<Account>(accountDto);

            var result = await _userManager.CreateAsync(user, accountDto.Password);

            if (result.Succeeded)
            {
                var userToReturn = _mapper.Map<AccountDto>(user);
                return userToReturn;
            }
            return null;
        }
        catch (System.Exception e)
        {

            throw new Exception($"Erro ao criar usuário. {e.Message}");
        }
    }

    public void DeleteImage(int userId, string imageName)
    {
        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/Images", imageName);
        if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
    }

    public async Task<AccountUpdateDto> GetUserByCredentialAsync(string credential)
    {
        var normalizedCredential = credential.ToUpper();

        var user = await _userManager.FindByNameAsync(normalizedCredential);
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(normalizedCredential);
            if (user == null) return null;
        }

        return _mapper.Map<AccountUpdateDto>(user);
    }

    public async Task<AccountUpdateDto> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await _accountPersist.GetUserByEmailAsync(email);
            if (user == null) return null;
            return _mapper.Map<AccountUpdateDto>(user);
        }
        catch (System.Exception e)
        {
            throw new Exception($"Erro ao obter usuário. {e.Message}");
        }
    }

    public async Task<AccountUpdateDto> GetUserByUsernameAsync(string username)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(username.ToLower());
            if (user == null) return null;
            return _mapper.Map<AccountUpdateDto>(user);
        }
        catch (System.Exception e)
        {

            throw new Exception($"Erro ao obter usuário. {e.Message}");
        }
    }

    public Task<bool> IsUsernameAvailable(string username)
    {
        var existingUser = _accountPersist.GetUserByUsernameAsync(username);
        return existingUser.Result == null ? Task.FromResult(true) : Task.FromResult(false);
    }

    public async Task<string> RemoveAccents(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        string normalizedString = input.Normalize(NormalizationForm.FormD);
        Regex regex = new Regex("[^a-zA-Z0-9 ]");
        return regex.Replace(normalizedString, "");
    }

    public async Task<string> SaveImage(IFormFile image)
    {
        string imageName = new String(Path.GetFileNameWithoutExtension(image.FileName)
                                        .Take(10).ToArray()).Replace(' ', '-');
        imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(image.FileName)}";

        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/Images", imageName);
        using (var fileStream = new FileStream(imagePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        return imageName;
    }

    public async Task<AccountUpdateDto> UpdateAccount(AccountUpdateDto accountUpdateDto, AccountUpdateDto outdatedUser)
    {
        try
        {
            var user = await _accountPersist.GetUserByUsernameAsync(outdatedUser.Username);
            if (user == null) return null;
            accountUpdateDto.Id = user.Id;
            _mapper.Map(accountUpdateDto, user);

            if (accountUpdateDto.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, accountUpdateDto.Password);
            }
            _accountPersist.Update<Account>(user);

            if (await _accountPersist.SaveChangesAsync())
            {
                var userToReturn = await _accountPersist.GetUserByUsernameAsync(accountUpdateDto.Username);
                return _mapper.Map<AccountUpdateDto>(userToReturn);
            }
            return null;
        }
        catch (System.Exception e)
        {

            throw new Exception($"Erro ao atualizar usuário. {e.Message}");
        }
    }

    public async Task<bool> UserExists(string username)
    {
        try
        {
            var normalizedUsername = await RemoveAccents(username);

            var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == normalizedUsername);
            if (existingUser != null)
            {
                return true;
            }

            return await _userManager.Users.AnyAsync(u => u.UserName == username);
        }
        catch (System.Exception e)
        {
            throw new Exception($"Erro ao verificar usuário. {e.Message}");
        }
    }

    public async Task<string> GenerateUniqueUsername(AccountDto accountDto)
    {
        accountDto.Username = accountDto.Username.Replace(" ", "");
        var baseUsername = await RemoveAccents(accountDto.Username);
        var username = baseUsername;
        var suffix = 1;
        while (await UserExists(username))
        {
            username = $"{baseUsername}{suffix}";
            suffix++;
        }

        return username;
    }

    private static string NormalizeUsername(string username)
    {
        return username.ToLower();
    }

    public bool DeleteAccountAsync(int id)
    {
        try
        {
            var user = _accountPersist.GetUserByIdAsync(id);
            if (user.Result == null) return false;
            _accountPersist.Delete<Account>(user.Result);
            if (_accountPersist.SaveChangesAsync().Result)
            {
                return true;
            }
            return false;
        }
        catch (System.Exception e)
        {
            throw new Exception($"Erro ao deletar usuário. {e.Message}");
        }
    }
}