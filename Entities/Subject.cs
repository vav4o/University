using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Entities
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        public int ProfessorId { get; set; }

        public string Title { get; set; }

        [ForeignKey("ProfessorId")]
        public virtual User Professor { get; set; }
    }
}
