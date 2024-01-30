using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace University.ViewModels.Subjects
{
    public class EditVM
    {
        public int Id { get; set; }
        public int ProfessorId { get; set; }

        [DisplayName("Subject Name: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Title { get; set; }
    }
}
