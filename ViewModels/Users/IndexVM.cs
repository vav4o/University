using University.Entities;
using University.ViewModels.Shared;

namespace University.ViewModels.Users
{
    public class IndexVm
    {
        public List<User> Items { get; set; }

        public PagerVM Pager { get; set; }
    }
}
