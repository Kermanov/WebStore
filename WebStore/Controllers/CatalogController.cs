using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.DTO;
using WebStore.Models;
using WebStore.Repositories;
using WebStore.Services;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ProductService productService;

        public CatalogController(ProductService productService)
        {
            this.productService = productService;
        }

        public IActionResult Index(FilterParams filterParams, int? pageNumber)
        {
            ViewBag.Categories = productService.GetCategories();

            IEnumerable<Product> products;
            if (filterParams != null)
            {
                products = productService.GetFiltered(filterParams);
                ViewBag.FilterParams = filterParams;
            }
            else
            {
                products = productService.GetAll();
            }

            ViewBag.PageNumber = pageNumber ?? 1;
            int pagesCount;
            products = productService.GetPaginated(products, pageNumber ?? 1, 1, out pagesCount);
            ViewBag.PagesCount = pagesCount;

            return View(products);
        }
    }
}