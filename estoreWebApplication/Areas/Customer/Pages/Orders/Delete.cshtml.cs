#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using estoreWebApplication.DataContext;
using estoreWebApplication.Models;

namespace estoreWebApplication.Areas.Customer.Pages.Orders
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly StoreDbContext _context;

        [BindProperty]
        public Order Order { get; set; }

        public DeleteModel(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Orders.FirstOrDefaultAsync(m => m.Id == id);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Orders.FindAsync(id);

            if (Order != null)
            {
                _context.Orders.Remove(Order);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}