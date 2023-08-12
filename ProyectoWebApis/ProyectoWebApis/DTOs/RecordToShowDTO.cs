using ProyectoWebApis.Models;

namespace ProyectoWebApis.DTOs
{
    public class RecordToShowDTO
    {
        public int Id { get; set; }
        public int Operation_Id { get; set; }
        public string User_Id { get; set; }
        public double User_Balance { get; set; }
        public double Ammount { get; set; }
        public string OperationResponse { get; set; }
        public DateTime DateTime { get; set; }
        public bool State { get; set; }
    }
}
