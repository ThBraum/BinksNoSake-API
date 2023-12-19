using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace BinksNoSake.Application.Dtos;
public class AccountLoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }

    public string? Provider { get; set; }
    public string? IdToken { get; set; }

    public string? ReturnUrl { get; set; }
    public IList<AuthenticationScheme>? ExternalLogins { get; set; }
}