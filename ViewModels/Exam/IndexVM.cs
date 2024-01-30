using University.Entities;
using University.ViewModels.Shared;

namespace University.ViewModels.Exams
{
    public class IndexVM
    {
        public List<Exam> Items { get; set; }

        public PagerVM Pager { get; set; }
    }
}
