using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            var products = productService.GetAll();
            return View(products);
        }
    }
}