using Microsoft.AspNetCore.Identity;

namespace BinksNoSake.Domain.Identity;
public class AccountRole : IdentityUserRole<int>
{
    public Account Account { get; set; }
    public Role Role { get; set; }
}