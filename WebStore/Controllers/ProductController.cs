using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.DTO;
using WebStore.Models;
using WebStore.Services;
using WebStore.UnitOfWork;

namespace WebStore.Controllers
{
    
    public class ProductController : Controller
    {
        private readonly ProductService productService;
        private readonly ImageService imageService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly WebStoreUnitOfWork unitOfWork;

        public ProductController(ProductService productService, ImageService imageService, UserManager<IdentityUser> userManager, WebStoreUnitOfWork unitOfWork)
        {
            this.productService = productService;
            this.imageService = imageService;
            _userManager = userManager;
            this.unitOfWork = unitOfWork;
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
                return View();
            }
        }

        // GET: Product/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            productService.Delete(id);
            return Ok();
        }

        // POST: Product/Delete/5
        [Authorize(Roles = "Admin")]
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

        [Authorize]
        public ActionResult AddToBasket(int product_id)
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            unitOfWork.ShoppingCartItems.Add(new CartItem { CartId = Guid.NewGuid().ToString() ,ProductId = product_id, UserId = user_id, Quantity = 1,Product = productService.GetById(product_id),ProductPrice = productService.GetById(product_id).Price });
            unitOfWork.Save();

            return RedirectToAction("Index","Catalog");
        }
        [Authorize]
        public ActionResult BasketItems()
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<CartItem> cartItems = unitOfWork.ShoppingCartItems.Select(x => x).Where(a => a.UserId == user_id);
            var model = cartItems.GroupBy(t => t.ProductId);
            List<CartItem> result = cartItems
                .GroupBy(l => l.Product.Id)
                .SelectMany(cl => cl.Select(
                    csLine => new CartItem
                    {
                        CartId = csLine.CartId,
                        UserId = csLine.UserId,
                        Quantity = cl.Count(),
                        Product = csLine.Product,
                        ProductPrice = cl.Sum(c => c.ProductPrice)
                    })).ToList<CartItem>();
            var res = result.Select(x => x.Product.Id).Distinct();
            List<CartItem> rrr = new List<CartItem>();
            foreach (var item in res)
            {
                rrr.Add(result.FirstOrDefault(x => x.Product.Id == item));
            }
            ViewBag.TotalSum = rrr.Sum(x => x.ProductPrice);
            return View(rrr.OrderBy(x=>x.ProductPrice));
        }
        [Authorize]
        public ActionResult RemoveOne(string id)
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = unitOfWork.ShoppingCartItems.FirstOrDefault(x => x.CartId == id);
            unitOfWork.ShoppingCartItems.Remove(cart);
            unitOfWork.Save();
            return RedirectToAction("BasketItems");
        }

        [Authorize]
        public ActionResult AddOne(string id)
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = unitOfWork.ShoppingCartItems.FirstOrDefault(x => x.CartId == id);
            unitOfWork.ShoppingCartItems.Add(new CartItem { CartId = Guid.NewGuid().ToString(), Product = cart.Product, ProductId = cart.Product.Id, ProductPrice = cart.ProductPrice, Quantity = 1, UserId = user_id});
            unitOfWork.Save();
            return RedirectToAction("BasketItems");
        }

        [Authorize]
        public ActionResult DeleteCart(int id)
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var carts = unitOfWork.ShoppingCartItems.Where(x => x.Product.Id == id);
            foreach (var cart in carts)
            {
                unitOfWork.ShoppingCartItems.Remove(cart);
                
            }
            unitOfWork.Save();
            return RedirectToAction("BasketItems");
        }
    }
}