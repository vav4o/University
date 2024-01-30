using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Entities
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int SubjectId { get; set; }
        public int ProfessorId { get; set; }
        public string Date { get; set; }

        [ForeignKey("ProfessorId")]
        public virtual User Professor { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
    }
}
