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
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.Categories = productService.GetCategories();
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
                return RedirectToAction("Index", "Catalog");
            }
        }

        // GET: Product/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            ViewBag.Product = productService.GetById(id);
            ViewBag.Categories = productService.GetCategories();

            return View();
        }

        // POST: Product/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductDTO productDTO)
        {
            try
            {
                var imageSource = productService.GetById(id).ImageSource;
                if (productDTO.Image != null)
                {
                    imageSource = imageService.SaveImage(productDTO.Image);
                }

                var productToUpdate = new Product
                {
                    Id = id,
                    Name = productDTO.Name,
                    Description = productDTO.Description,
                    ImageSource = imageSource,
                    Price = productDTO.Price,
                    DisplayComments = productDTO.DisplayComments,
                    CategoryId = productDTO.CategoryId
                };

                productService.Update(productToUpdate);

                return RedirectToAction("Details", new { id });
            }
            catch
            {
                return RedirectToAction("Index", "Catalog");
            }
        }

        // GET: Product/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            productService.Delete(id);
            return RedirectToAction("Index", "Catalog");
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