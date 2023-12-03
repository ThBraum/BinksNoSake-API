using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinksNoSake.Domain.Models;

namespace BinksNoSake.Persistence.Contratos;
public interface ITokenPersit
{
    Task<string> GetRefreshToken(string username);
    void DeleteRefreshToken(string username, string refreshToken);
}