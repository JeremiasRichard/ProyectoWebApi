using System.ComponentModel.DataAnnotations;

namespace ProyectoWebApis.DTOs
{
    public class OperationToShowDTO
    {
        [Required] public string OperationName { get; set; }

        [Required]
        public double Cost { get; set; }
    }
}
