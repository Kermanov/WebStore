using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;
using WebStore.Repositories;
using WebStore.UnitOfWork;

namespace WebStore.Services
{
    public class ProductService
    {
        private readonly WebStoreUnitOfWork unitOfWork;

        public ProductService(WebStoreUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Product> GetAll()
        {
            return unitOfWork.Products.GetAll();
        }

        public Product GetById(int id)
        {
            return unitOfWork.Products.GetById(id);
        }

        public void Create(Product product)
        {
            unitOfWork.Products.Create(product);
            unitOfWork.Save();
        }

        public IEnumerable<Category> GetCategories()
        {
            return unitOfWork.Categories.GetAll();
        }

        public void Delete(int id)
        {
            unitOfWork.Products.DeleteById(id);
            unitOfWork.Save();
        }
    }
}
