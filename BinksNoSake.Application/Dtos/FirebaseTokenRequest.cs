using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinksNoSake.Application.Dtos;
public class FirebaseTokenRequest
{
    public string? FireBaseToken { get; set; }
    public string? idToken { get; set; }
}