using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinksNoSake.Application.Dtos;
public class AccountUpdateDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PrimeiroNome { get; set; }
    public string UltimoNome { get; set; }
    public string PhoneNumber { get; set; }
    public string ImagemURL { get; set; }
    public string Funcao { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
    public IEnumerable<string> UserRoles { get; set; }
}