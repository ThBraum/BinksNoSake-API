using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinksNoSake.API.Helpers;
public interface IUtil
{
    Task<string> SaveImage(IFormFile imageFile, string destino = "Images");
    void DeleteImage(string imageName, string destino = "Images");
}