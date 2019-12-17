using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class OAuthController : Controller
    {

        public ActionResult SignFacebook()
        {
            return View();
        }
        // GET: OAuth
        public ActionResult Index()
        {
            return View();
        }

        // GET: OAuth/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OAuth/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OAuth/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OAuth/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OAuth/Edit/5
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

        // GET: OAuth/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OAuth/Delete/5
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