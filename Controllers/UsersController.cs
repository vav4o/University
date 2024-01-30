using Microsoft.AspNetCore.Mvc;
using University.ActionFilters;
using University.Entities;
using University.Repositories;
using University.ViewModels.Shared;
using University.ViewModels.Users;

namespace University.Controllers
{
    public class UsersController : Controller
    {
        [HttpGet]
        public IActionResult Index(IndexVm model)
        {
            UniversityDBContext context = new UniversityDBContext();

            model.Pager = model.Pager ?? new PagerVM(); 
            model.Pager.Page = model.Pager.Page <= 0 ? 1 : model.Pager.Page;
            model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0 ? 10 : model.Pager.ItemsPerPage;

            model.Pager.PageCount = (int)Math.Ceiling(context.Users.Count() / (double)model.Pager.ItemsPerPage);

            model.Items = context.Users.OrderBy(i => i.Id)
                                       .Skip(model.Pager.ItemsPerPage * (model.Pager.Page - 1))
                                       .Take(model.Pager.ItemsPerPage)
                                       .ToList();

            return View(model);
        }
        [HttpGet]
        public IActionResult Create() 
        {
            CreateVM model = new CreateVM();
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateVM model)
        {           
            if (!ModelState.IsValid) return View(model);   

            User item = new User();
            
            item.Username = model.Username;
            item.Password = model.Password;
            item.FirstName = model.FirstName; 
            item.LastName = model.LastName;

            UniversityDBContext context = new UniversityDBContext();

            context.Users.Add(item);
            context.SaveChanges();

            return RedirectToAction("Index", "Users");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            UniversityDBContext context = new UniversityDBContext();
            User item = context.Users.Where(x => x.Id == id).FirstOrDefault();

            if (item == null) return RedirectToAction("Index", "Users");

            EditVM model = new EditVM();
            model.Id = item.Id;
            model.Username = item.Username;
            model.Password = item.Password;
            model.FirstName = item.FirstName;
            model.LastName = item.LastName;

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditVM model)
        {
            if (!ModelState.IsValid) return View(model);

            User item = new User();
           
            item.Id = model.Id;
            item.Username = model.Username;
            item.Password = model.Password;
            item.FirstName = model.FirstName;
            item.LastName = model.LastName;

            UniversityDBContext context = new UniversityDBContext();

            context.Users.Update(item);
            context.SaveChanges();

            return RedirectToAction("Index", "Users");
        }

        [HttpGet]
        public IActionResult Delete(int id) 
        {
            UniversityDBContext context = new UniversityDBContext();
            User item = new User();
            item.Id = id;

            context.Users.Remove(item);
            context.SaveChanges();

            return RedirectToAction("Index", "Users");
        }
    }
}
