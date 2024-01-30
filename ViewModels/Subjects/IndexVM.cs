using University.Entities;
using University.ViewModels.Shared;

namespace University.ViewModels.Subjects
{
    public class IndexVM
    {
        public List<Subject> Items { get; set; }

        public PagerVM Pager { get; set; }
    }
}
