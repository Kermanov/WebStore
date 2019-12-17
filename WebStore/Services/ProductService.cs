using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DTO;
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

        public IEnumerable<Product> GetFiltered(FilterParams filterParams)
        {
            var products = unitOfWork.Products.GetAll();

            if (!string.IsNullOrWhiteSpace(filterParams.NameFilter))
            {
                products = products.Where(product => 
                    product.Name.Contains(filterParams.NameFilter, StringComparison.OrdinalIgnoreCase)
                );
            }

            if (filterParams.CategoryFilter > 0)
            {
                products = products.Where(product => product.CategoryId == filterParams.CategoryFilter);
            }

            if (filterParams.MinPrice > 0)
            {
                products = products.Where(product => product.Price >= filterParams.MinPrice);
            }

            if (filterParams.MaxPrice > 0)
            {
                products = products.Where(product => product.Price <= filterParams.MaxPrice);
            }

            if (filterParams.SortParameter == SortParameter.ExpensiveFirst)
            {
                products = products.OrderByDescending(product => product.Price);
            }
            else if (filterParams.SortParameter == SortParameter.CheapFirst)
            {
                products = products.OrderByDescending(product => product.Price).Reverse();
            }

            return products;
        }

        public IEnumerable<Product> GetPaginated(IEnumerable<Product> products, int pageNumber, int pageSize, out int pagesCount)
        {
            pagesCount = products.Count() / pageSize + 1;

            var productPage = new List<Product>();

            int firstIndex = pageSize * (pageNumber - 1);
            for (int i = firstIndex; i < firstIndex + pageSize && i < products.Count(); ++i)
            {
                productPage.Add(products.ElementAt(i));
            }

            return productPage;
        }

        public IEnumerable<Comment> GetComments(int productId)
        {
            return unitOfWork.Comments.Find(comment => comment.ProductId == productId);
        }

        public void AddComment(Comment comment)
        {
            unitOfWork.Comments.Create(comment);
            unitOfWork.Save();
        }
    }
}
