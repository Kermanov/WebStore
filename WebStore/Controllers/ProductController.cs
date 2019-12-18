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
            IEnumerable<CartItem> cartItems = unitOfWork.ShoppingCartItems.Select(x => x).Where(a => a.UserId == user_id && a.Buyed == false);
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

        [Authorize]
        public ActionResult SubmitCart()
        {
            
            return RedirectToAction("BasketItems");
        }
        [Authorize]
        public ActionResult SubmitDelivery()
        {
            return View("SubmitDelivery");
        }

        [Authorize]
        [HttpPost]
        public ActionResult SubmitDelivery(string Country, string City, string Pochta)
        {
            if(string.IsNullOrEmpty(Country))
            {
                ModelState.AddModelError("Country", "Please enter a country");
            }
            if (string.IsNullOrEmpty(City))
            {
                ModelState.AddModelError("City", "Please enter a City");
            }
            if (string.IsNullOrEmpty(Pochta))
            {
                ModelState.AddModelError("Pochta", "Please enter a Pochta");
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction("AreYouSure",new { Country = Country, City = City, Pochta = Pochta});
            }
            else
            {
                return View();
            }
        }
        [Authorize]
        public ActionResult AreYouSure(IEnumerable<CartItem> cartItems, string Country, string City, string Pochta)
        {
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            cartItems = unitOfWork.ShoppingCartItems.Select(x => x).Where(a => a.UserId == user_id && a.Buyed == false);
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
            Delivery d = new Delivery { DeliveryId = Guid.NewGuid().ToString(), Country = Country, City = City, Pochta = Pochta, Quantity = result.Count(), Product = new Product(), ProductId = 0, UserId = user_id };
            var t = new Tuple<IEnumerable<CartItem>, Delivery>(rrr, d);
            TempData["Country"] = Country;
            TempData["City"] = City;
            TempData["Pochta"] = Pochta;
            return View(t);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AreYouSure()
        {
            string Country = TempData["Country"].ToString();
            string City = TempData["City"].ToString();
            string Pochta = TempData["Pochta"].ToString();
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<CartItem> cartItems = unitOfWork.ShoppingCartItems.Select(x => x).Where(a => a.UserId == user_id && a.Buyed == false);
            foreach (var item in cartItems)
            {
                item.Buyed = true;
            }
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
            foreach (var item in rrr)
            {
                Delivery delivery = new Delivery
                {
                    DeliveryId = Guid.NewGuid().ToString(),
                    UserId = user_id,
                    Quantity = item.Quantity,
                    Country = Country,
                    ProductId = item.Product.Id,
                    City = City,
                    Pochta = Pochta
                };
                unitOfWork.Deliveries.Add(delivery);
            }
            unitOfWork.Save();
            return RedirectToAction("Index", "Catalog");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteComment(int commentId, int productId)
        {
            productService.DeleteComment(commentId);

            return RedirectToAction("Details", "Product", new { id = productId });
        }
    }
}