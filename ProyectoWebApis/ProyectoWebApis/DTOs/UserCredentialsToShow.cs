using System.ComponentModel.DataAnnotations;

namespace ProyectoWebApis.DTOs
{
    public class UserCredentialsToShow
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
