using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.ExtensionMethods;
using University.Entities;
using University.Repositories;
using University.ViewModels.Home;
using University.ActionFilters;

namespace University.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            if (!this.ModelState.IsValid) return View(model);

            UniversityDBContext context = new UniversityDBContext();
            
            User loggedUser = context.Users.Where(x => x.Username == model.Username && x.Password == model.Password).FirstOrDefault();

            if (loggedUser == null) 
            {
                this.ModelState.AddModelError("authError", "Invalid username or password!");
                return View(model);
            }

            HttpContext.Session.SetObject<User>("loggedUser", loggedUser);

            return RedirectToAction("Index", "Home");
        }

        [AuthenticationFilter]
        public ActionResult Logout() 
        {
            HttpContext.Session.Remove("loggedUser");

            return RedirectToAction("Login", "Home");
        }
    }
}
