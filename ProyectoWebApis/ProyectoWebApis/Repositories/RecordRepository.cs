using Microsoft.EntityFrameworkCore;
using ProyectoWebApis.DataBase;
using ProyectoWebApis.Interfaces;
using ProyectoWebApis.Models;

namespace ProyectoWebApis.Repositories
{
    public class RecordRepository : IRecord
    {
        private readonly ApplicationDbContext _context;

        public RecordRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public bool Create(Record record)
        {
            _context.Add(record);
            return Save();
        }

        public bool Exist(int id)
        {
            return _context.Records.Any(c => c.Id == id);
        }

        public ICollection<Record> GetAll()
        {
            return _context.Records.ToList();
        }

        public Record GetById(int id)
        {
            return _context.Records.Where(x => x.Id == id).FirstOrDefault();
        }

        public bool Remove(Record record)
        {
            _context.Remove(record);
            return Save();
        }

        public bool Save()
        {
           var saved = _context.SaveChanges();
           return saved > 0;
        }

        public bool SoftDeleted(int id)
        {
            Record toUpdate = GetById(id);
            toUpdate.State = false;
            return Update(toUpdate);
        }

        public bool Update(Record record)
        {
            _context.Update(record);
            return Save();
        }
    }
}
