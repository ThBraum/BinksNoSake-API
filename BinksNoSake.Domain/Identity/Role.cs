using Microsoft.AspNetCore.Identity;

namespace BinksNoSake.Domain.Identity;
public class Role : IdentityRole<int>
{
    public IEnumerable<AccountRole> AccountRole { get; set; }
}