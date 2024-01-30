using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace University.ViewModels.Subjects
{
    public class CreateVM
    {
        public int ProfessorId { get; set; }

        [DisplayName("Subject Name: ")]
        [Required(ErrorMessage ="*This field is Required!")]
        public string Title { get; set; }
    }
}
