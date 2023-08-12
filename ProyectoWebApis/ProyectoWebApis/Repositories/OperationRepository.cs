using ProyectoWebApis.DataBase;
using ProyectoWebApis.Interfaces;
using ProyectoWebApis.Models;

namespace ProyectoWebApis.Repositories
{
    public class OperationRepository : IOperation
    {
        private readonly ApplicationDbContext _dbContext;

        public OperationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Create(Operation operation)
        {
            _dbContext.Add(operation);
            return Save();
        }

        public bool Exist(int id)
        {
            return _dbContext.Operations.Any(x => x.Id == id);
        }

        public ICollection<Operation> GetAll()
        {
            return _dbContext.Operations.ToList();
        }

        public Operation GetById(int id)
        {
          return _dbContext.Operations.Where(x => x.Id == id).FirstOrDefault();
        }

        public bool Remove(Operation operation)
        {
            _dbContext.Remove(operation);
            return Save();
        }

        public bool Save()
        {
            var saved = _dbContext.SaveChanges();
            return saved > 0;
        }

        public bool Update(Operation operation)
        {
            _dbContext.Update(operation);
            return Save();
        }
    }
}
