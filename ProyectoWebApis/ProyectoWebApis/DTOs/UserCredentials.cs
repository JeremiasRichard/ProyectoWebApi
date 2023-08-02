using System.ComponentModel.DataAnnotations;

namespace ProyectoWebApis.DTOs
{
    public class UserCredentials
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public double Balance { get; set; }

    }
}
