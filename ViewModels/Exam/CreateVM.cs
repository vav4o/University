using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using University.Entities;

namespace University.ViewModels.Exams
{
    public class CreateVM
    {
        public int ProfessorId { get; set; }
        public int SubjectId { get; set; }

        [DisplayName("Exam Name: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Name { get; set; }

        [DisplayName("Date: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Date { get; set; }

        public List<Subject> Subjects { get; set; }
    }
}
