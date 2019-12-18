using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class ContactController : Controller
    {
        public void SendEmail(string email_to,string email_from, string message)
        {

            MailAddress from = new MailAddress("samarnazar33@gmail.com", "Web-Shop");

            MailAddress to = new MailAddress(email_to);

            MailMessage m = new MailMessage(from, to);

            m.Subject = "ContactUs Message from "+ email_from;

            m.Body = $"<body><h2>{message}</h2></body>";

            m.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

            smtp.Credentials = new NetworkCredential("samarnazar33@gmail.com", "studvoice123");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string Email, string Message)
        {
            if(string.IsNullOrEmpty(Message))
            {
                ModelState.AddModelError("Message", "Message cannot be empty");
                return View();
            }
            if (ModelState.IsValid)
            {
                SendEmail("nazarsamar32@gmail.com", Email, Message);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}