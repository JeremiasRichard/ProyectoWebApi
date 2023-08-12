using ProyectoWebApis.Models;
using System.ComponentModel.DataAnnotations;

namespace ProyectoWebApis.DTOs
{
    public class RecordCreateDTO
    {
        [Required]   
        public int Operation_Id { get; set; }
        [Required]
        public string NumberOne { get; set; }
        [Required]
        public string NumberTwo { get; set; }
    }
}
