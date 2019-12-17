using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Interfaces;
using WebStore.Models;

namespace WebStore.Repositories
{
    public class Repository<T> : IRepository<T> where T: BaseModel
    {
        private readonly ApplicationDbContext context;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Create(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public void DeleteById(int id)
        {
            var entityToDelete = context.Set<T>().Find(id);
            if (entityToDelete != null)
            {
                context.Set<T>().Remove(entityToDelete);
            }
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return context.Set<T>().Where(predicate).ToList();
        }

        public IEnumerable<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return context.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            var entityToUpdate = context.Set<T>().Find(entity.Id);
            if (entityToUpdate != null)
            {
                context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            }
        }
    }
}
