#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using estoreWebApplication.DataContext;
using estoreWebApplication.Models;

namespace estoreWebApplication.Areas.Admin.Pages.Orders
{
    [Authorize(Policy = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly StoreDbContext _context;

        public IList<Order> Orders { get; set; }

        public IndexModel(DataContext.StoreDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Orders = await _context.Orders
                .OrderByDescending(o => o.Date)
                .ToListAsync();
        }
    }
}
