using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Context;
using WebStore.Models;
using WebStore.Repositories;

namespace WebStore.UnitOfWork
{
    public class WebStoreUnitOfWork: IDisposable
    {
        private readonly WebStoreContext context;
        private bool disposed = false;

        public WebStoreUnitOfWork(WebStoreContext context)
        {
            this.context = context;

            Categories = new Repository<Category>(context);
            Products = new Repository<Product>(context);
        }

        public Repository<Category> Categories { get; }
        public Repository<Product> Products { get; }

        public void Save()
        {
            context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
