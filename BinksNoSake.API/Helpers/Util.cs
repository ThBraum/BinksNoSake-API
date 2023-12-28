using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinksNoSake.Application.Contratos;

namespace BinksNoSake.API.Helpers;
public class Util : IUtil
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IAccountService _accountService;
    public Util(IWebHostEnvironment hostEnvironment, IAccountService accountService)
    {
        _hostEnvironment = hostEnvironment;
        _accountService = accountService;
    }


    public void DeleteImage(string imageName, string destino = "Images")
    {
        if (!string.IsNullOrEmpty(imageName))
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources/{destino}", imageName);
            if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
        }
    }

    public async Task<string> SaveImage(IFormFile imageFile, string destino = "Images")
    {
        string imageName = await ObterNomeDaImagem(imageFile.FileName);

        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources/{destino}", imageName);

        using (var fileStream = new FileStream(imagePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
            fileStream.Seek(0, SeekOrigin.Begin); 
        }

        return imageName;
    }

    private async Task<string> ObterNomeDaImagem(string fileName)
    {
        string nomeSemExtensao = Path.GetFileNameWithoutExtension(fileName);
        nomeSemExtensao = nomeSemExtensao.Replace(' ', '-');

        string imageName = $"{nomeSemExtensao}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(fileName)}";

        return imageName;
    }
}