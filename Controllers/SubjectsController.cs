using Microsoft.AspNetCore.Mvc;
using University.ActionFilters;
using University.Entities;
using University.ExtensionMethods;
using University.Repositories;
using University.ViewModels.Shared;
using University.ViewModels.Subjects;

namespace University.Controllers
{
    public class SubjectsController : Controller
    {
        [AuthenticationFilter]
        public IActionResult Index(IndexVM model)
        {
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");

            UniversityDBContext context = new UniversityDBContext();

            model.Pager = model.Pager ?? new PagerVM();
            model.Pager.Page = model.Pager.Page <= 0 ? 1 : model.Pager.Page;
            model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0 ? 10 : model.Pager.ItemsPerPage;

            List<int> sharedSubjectIds = context.UserToSubjects.Where(i => i.UserId == loggedUser.Id)
                                                               .Select(i => i.SubjectId)
                                                               .ToList();
            model.Pager.PageCount = (int)Math.Ceiling(context.Subjects.Where
                (x => x.ProfessorId == loggedUser.Id || sharedSubjectIds.Contains(x.Id)).Count() 
                / (double)model.Pager.ItemsPerPage);

            model.Items = context.Subjects.Where(x => x.ProfessorId == loggedUser.Id || sharedSubjectIds
                                          .Contains(x.Id))
                                          .ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");

            CreateVM model = new CreateVM();
            model.ProfessorId = loggedUser.Id;

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateVM model)
        {
            UniversityDBContext context = new UniversityDBContext();
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");

            if (model.ProfessorId != loggedUser.Id) 
            {
                ModelState.AddModelError("ownerError", "Wrong subject owner!");
            }

            Subject item = new Subject();
            item.ProfessorId = model.ProfessorId;
            item.Title = model.Title;

            context.Subjects.Add(item);
            context.SaveChanges();

            return RedirectToAction("Index", "Subjects");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");
            UniversityDBContext context = new UniversityDBContext(); 
            Subject item = context.Subjects.Where(x => x.Id == id).FirstOrDefault();

            if (item == null || item.ProfessorId != loggedUser.Id) return RedirectToAction("Index", "Subjects");

            EditVM model = new EditVM();
            model.Id = item.Id;
            model.Title = item.Title;
            model.ProfessorId = item.ProfessorId;

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditVM model) 
        {
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");
            UniversityDBContext context = new UniversityDBContext();
            Subject item = context.Subjects.Where(x => x.Id == model.Id).FirstOrDefault();

            if (item.ProfessorId != loggedUser.Id)
            {
                ModelState.AddModelError("ownerError", "Wrong subject owner!");
                return View(model);
            }

            item.Id = model.Id;
            item.Title = model.Title;
            item.ProfessorId = model.ProfessorId;

            context.Subjects.Update(item);
            context.SaveChanges();

            return RedirectToAction("Index", "Subjects");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            UniversityDBContext context = new UniversityDBContext();
            Subject item = new Subject();
            item.Id = id;

            context.Subjects.Remove(item);
            context.SaveChanges();

            return RedirectToAction("Index", "Subjects");
        }

        [HttpGet]
        public IActionResult Share(int Id)
        {
            UniversityDBContext context = new UniversityDBContext();
            ShareVM model = new ShareVM();
            model.Subject = context.Subjects.Where(x => x.Id == Id).FirstOrDefault();
            model.Shares = context.UserToSubjects.Where(x => x.SubjectId == Id).ToList();

            List<int> usersSharedList = model.Shares.Select(x => x.UserId).ToList();

            usersSharedList.Add(model.Subject.ProfessorId);
            model.Users = context.Users.Where(x => !usersSharedList.Contains(x.Id)).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Share(ShareVM model) 
        {
            UniversityDBContext context = new UniversityDBContext();

            foreach (int userId in model.UserIds)
            {
                UserToSubject item = new UserToSubject();
                item.UserId = userId;
                item.SubjectId = model.SubjectId;

                context.UserToSubjects.Add(item);
                context.SaveChanges();
            }

            return RedirectToAction("Share", "Subjects", new { Id = model.SubjectId });
        }

        [HttpGet]
        public IActionResult RevokeShare(int id) 
        {
            UniversityDBContext context = new UniversityDBContext();

            UserToSubject item = context.UserToSubjects.Where(x => x.Id == id).FirstOrDefault();

            context.UserToSubjects.Remove(item);
            context.SaveChanges();

            return RedirectToAction("Share", "Subjects", new { Id = item.SubjectId });
        }
    }
}
