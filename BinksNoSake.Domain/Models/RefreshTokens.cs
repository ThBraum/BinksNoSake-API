using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinksNoSake.Domain.Models;
public class RefreshTokens
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}