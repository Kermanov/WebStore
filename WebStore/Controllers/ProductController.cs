using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.DTO;
using WebStore.Models;
using WebStore.Services;

namespace WebStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService productService;
        private readonly ImageService imageService;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductController(ProductService productService, ImageService imageService, UserManager<IdentityUser> userManager)
        {
            this.productService = productService;
            this.imageService = imageService;
            _userManager = userManager;
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
            var comments = productService.GetComments(id);
            ViewBag.Comments = comments;

            return View(product);
        }

        // GET: Product/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Categories = productService.GetCategories();
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(ProductDTO productDto)
        {
            try
            {
                var imageSource = imageService.SaveImage(productDto.Image);
                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    DisplayComments = productDto.DisplayComments,
                    CategoryId = productDto.CategoryId,
                    ImageSource = imageSource
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
            productService.Delete(id);
            return Ok();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CreateComment(CommentDTO commentDTO)
        {
            var newComment = new Comment
            {
                UserId = _userManager.GetUserId(User),
                ProductId = commentDTO.ProductId,
                CommentText = commentDTO.CommentText
            };

            productService.AddComment(newComment);

            return RedirectToAction("Details", new { id = commentDTO.ProductId });
        }
    }
}