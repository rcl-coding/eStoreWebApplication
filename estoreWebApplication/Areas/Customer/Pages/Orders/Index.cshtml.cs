#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using estoreWebApplication.DataContext;
using estoreWebApplication.Models;

namespace estoreWebApplication.Areas.Customer.Pages.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly StoreDbContext _context;

        public IList<Order> Orders { get; set; }

        public IndexModel(StoreDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            UserViewModel user = Helpers.UserClaims.GetUserClaims(User);
            Orders = await _context.Orders
                .Where(w => w.CustomerEmail == user.Email)
                .OrderByDescending(o => o.Date)
                .ToListAsync();
        }
    }
}
