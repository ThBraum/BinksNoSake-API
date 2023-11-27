using BinksNoSake.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace BinksNoSake.Domain.Identity;
public class Account : IdentityUser<int>
{
    public string PrimeiroNome { get; set; }
    public string UltimoNome { get; set; }
    public Funcao Funcao { get; set; }
    public string ImagemURL { get; set; }
    public IEnumerable<AccountRole> AccountRoles { get; set; }

}