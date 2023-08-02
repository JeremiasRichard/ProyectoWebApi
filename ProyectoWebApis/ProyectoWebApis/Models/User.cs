using Microsoft.AspNetCore.Identity;

namespace ProyectoWebApis.Models
{
    public class User : IdentityUser
    {
       public bool Status { get;set; }

    }
}
