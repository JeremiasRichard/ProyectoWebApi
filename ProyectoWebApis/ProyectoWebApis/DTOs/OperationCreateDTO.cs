using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ProyectoWebApis.DTOs
{
    public class OperationCreateDTO
    {
        [Required]
        public OperationType Type { get; set; }
        [Required] public string OperationName { get; set; }

        [Required]
        public double Cost { get; set; }
    }
}
