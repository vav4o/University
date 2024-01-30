using Microsoft.EntityFrameworkCore.Query;
using University.Entities;

namespace University.ViewModels.Subjects
{
    public class ShareVM
    {
        public int SubjectId { get; set; }
        public List<int> UserIds { get; set; }

        public Subject Subject { get; set; }
        public List<UserToSubject> Shares { get; set; }
        public List<User> Users { get; set; }
    }
}
