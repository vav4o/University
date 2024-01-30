using Microsoft.AspNetCore.Mvc;
using University.ActionFilters;
using University.Entities;
using University.ExtensionMethods;
using University.Repositories;
using University.ViewModels.Exams;
using University.ViewModels.Shared;

namespace University.Controllers
{
    public class ExamController : Controller
    {
        public IActionResult Index(IndexVM model)
        {
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");

            UniversityDBContext context = new UniversityDBContext();

            model.Pager = model.Pager ?? new PagerVM();
            model.Pager.Page = model.Pager.Page <= 0 ? 1 : model.Pager.Page;
            model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0 ? 10 : model.Pager.ItemsPerPage;

            model.Pager.PageCount = (int)Math.Ceiling(context.Exams.Count() / (double)model.Pager.ItemsPerPage);

            model.Items = context.Exams.OrderBy(i => i.Id)
                                       .Skip(model.Pager.ItemsPerPage * (model.Pager.Page - 1))
                                       .Take(model.Pager.ItemsPerPage)
                                       .ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            UniversityDBContext context = new UniversityDBContext();
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");

            CreateVM model = new CreateVM();
            model.ProfessorId = loggedUser.Id;
            List<int> sharedSubjectIds = context.UserToSubjects.Where(i => i.UserId == loggedUser.Id)
                                                               .Select(i => i.SubjectId)
                                                               .ToList();
            model.Subjects = context.Subjects.Where(x => x.ProfessorId == loggedUser.Id || sharedSubjectIds
                                          .Contains(x.Id)).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateVM model)
        {
            UniversityDBContext context = new UniversityDBContext();
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");

            if (model.ProfessorId != loggedUser.Id)
            {
                ModelState.AddModelError("ownerError", "Wrong exam owner!");
            }

            Exam item = new Exam();
            item.ProfessorId = model.ProfessorId;
            item.Name = model.Name;
            item.Date = model.Date;
            item.SubjectId = model.SubjectId;

            context.Exams.Add(item);
            context.SaveChanges();

            return RedirectToAction("Index", "Exam");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");
            UniversityDBContext context = new UniversityDBContext();
            Exam item = context.Exams.Where(x => x.Id == id).FirstOrDefault();

            if (item == null || item.ProfessorId != loggedUser.Id) return RedirectToAction("Index", "Exams");

            EditVM model = new EditVM();
            model.Id = item.Id;
            model.Name = item.Name;
            model.Date = item.Date;
            model.ProfessorId = item.ProfessorId;
            model.SubjectId = item.SubjectId;
            model.Subjects = context.Subjects.ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditVM model)
        {
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");
            UniversityDBContext context = new UniversityDBContext();
            Exam item = context.Exams.Where(x => x.Id == model.Id).FirstOrDefault();

            if (item.ProfessorId != loggedUser.Id)
            {
                ModelState.AddModelError("ownerError", "Wrong exam owner!");
                return View(model);
            }

            item.Id = model.Id;
            item.Name = model.Name;
            item.ProfessorId = model.ProfessorId;
            item.SubjectId = model.SubjectId;

            context.Exams.Update(item);
            context.SaveChanges();

            return RedirectToAction("Index", "Exam");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            UniversityDBContext context = new UniversityDBContext();
            Exam item = new Exam();
            item.Id = id;

            context.Exams.Remove(item);
            context.SaveChanges();

            return RedirectToAction("Index", "Exam");
        }
    }
}
