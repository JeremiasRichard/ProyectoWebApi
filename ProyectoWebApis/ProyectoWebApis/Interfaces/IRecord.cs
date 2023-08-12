using ProyectoWebApis.Models;

namespace ProyectoWebApis.Interfaces
{
    public interface IRecord
    {

        bool Create(Record record);
        bool Update(Record record);
        bool Remove(Record record);
        bool Exist(int id);
        bool Save();
        public Record GetById(int id);
        public ICollection<Record> GetAll();
        public bool SoftDeleted(int id);
    }
}
