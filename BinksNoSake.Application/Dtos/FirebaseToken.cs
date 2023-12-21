using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinksNoSake.Application.Dtos;
public class FirebaseToken
{
    public string UserId { get; set; }
    public string FireBaseToken { get; set; }
    public string acessToken { get; set; }
    public string idToken { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string GivenName { get; set; }
    public string Family_name { get; set; }
    public string Granted_Scopes { get; set; }
    public string Picture { get; set; }
}