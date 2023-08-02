namespace ProyectoWebApis.Models
{   
  
    public class Operation
    {
        public enum OperationType
        {
            ADDITION,
            SUBSTRACTION,
            MULTIPLICATION,
            DIVISION,
            SQUARE_ROOT,
            RANDOM_STRING
        }

        public int Id { get; set; }
        public OperationType Type { get; set; }
        public double Cost { get; set; }
    }
}
