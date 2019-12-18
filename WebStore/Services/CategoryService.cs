using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;
using WebStore.UnitOfWork;

namespace WebStore.Services
{
    public class CategoryService
    {
        private readonly WebStoreUnitOfWork unitOfWork;

        public CategoryService(WebStoreUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void AddCategory(Category category)
        {
            unitOfWork.Categories.Create(category);
            unitOfWork.Save();
        }

        public void DeleteCategory(int id)
        {
            unitOfWork.Categories.DeleteById(id);
            unitOfWork.Save();
        }

        public IEnumerable<Category> GetAll()
        {
            return unitOfWork.Categories.GetAll();
        }
    }
}
