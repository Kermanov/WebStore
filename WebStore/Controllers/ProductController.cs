using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.DTO;
using WebStore.Models;
using WebStore.Services;

namespace WebStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService productService;

        public ProductController(ProductService productService)
        {
            this.productService = productService;
        }

        // GET: Product
        public IActionResult Index()
        {
            return View();
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            var product = productService.GetById(id);
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.Categories = productService.GetCategories();
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductDTO productDto)
        {
            try
            {
                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    DisplayComments = productDto.DisplayComments,
                    CategoryId = productDto.CategoryId
                };
                productService.Create(product);
                return RedirectToAction("Details", new { product.Id });
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}