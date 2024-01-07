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
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string? ImagemURL { get; set; }
}