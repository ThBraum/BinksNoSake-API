using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BinksNoSake.Application.Dtos;
public class AccountUpdateDto
{
    public int? Id { get; set; }
    public string?  Username { get; set; }
    public string? Email { get; set; }
    public string? Nome { get; set; }
    public string? Sobrenome { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ImagemURL { get; set; }
    public string? Funcao { get; set; }
    public string? Password { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public IEnumerable<string>?  UserRoles { get; set; }
}