using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinksNoSake.Application.Dtos;
public class AccountDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PrimeiroNome { get; set; }
    public string UltimoNome { get; set; }
}