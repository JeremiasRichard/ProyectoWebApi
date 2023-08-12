using System.ComponentModel.DataAnnotations;

namespace ProyectoWebApis.DTOs
{
    public class UserPermissionsCredentials
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
