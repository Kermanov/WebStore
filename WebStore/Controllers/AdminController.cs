using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Services;

namespace WebStore.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly UserService userService;

        public AdminController(UserManager<IdentityUser> userManager, UserService userService)
        {
            this.userManager = userManager;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult ManageUsers()
        {
            return View(userService.GetUsersWithRoles());
        }

        public async Task<ActionResult> RemoveRights(string userId)
        {
            var user = userService.GetUser(userId);
            await userManager.RemoveFromRoleAsync(user, "Admin");
            return RedirectToAction("ManageUsers");
        }

        public async Task<ActionResult> GiveRights(string userId)
        {
            var user = userService.GetUser(userId);
            await userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction("ManageUsers");
        }
    }
}