using Domain.Models;

namespace jwtOigaSln.ViewModels
{
    public class ResultViewModel
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
