using BinksNoSake.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace BinksNoSake.Domain.Identity;
public class Account : IdentityUser<int>
{
    public string Username { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    // public string Email { get; set; }
    public Funcao Funcao { get; set; }
    public string ImagemURL { get; set; }
    public IEnumerable<AccountRole> AccountRoles { get; set; }

}