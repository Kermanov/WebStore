using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Interfaces
{
    public interface IRepository<T> where T: BaseModel
    {
        void Create(T entity);
        void Update(T entity);
        IEnumerable<T> GetAll();
        T GetById(int id);
        void DeleteById(int id);
        IEnumerable<T> Find(Func<T, bool> predicate);
    }
}
