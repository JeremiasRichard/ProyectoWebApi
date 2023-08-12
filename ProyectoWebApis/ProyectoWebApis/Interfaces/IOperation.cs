using ProyectoWebApis.Models;

namespace ProyectoWebApis.Interfaces
{
    public interface IOperation
    {
        bool Create(Operation operation);
        bool Update(Operation operation);
        bool Remove(Operation operation);
        bool Exist(int id);
        bool Save();
        public Operation GetById(int id);
        public ICollection<Operation> GetAll();
    }
}
