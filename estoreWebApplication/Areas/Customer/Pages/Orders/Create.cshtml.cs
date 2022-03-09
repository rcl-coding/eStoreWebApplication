#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using estoreWebApplication.DataContext;
using estoreWebApplication.Models;

namespace estoreWebApplication.Areas.Customer.Pages.Orders
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly StoreDbContext _context;

        public Product Product { get; set; }

        [BindProperty]
        public Order Order { get; set; }

        public CreateModel(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? productId)
        {
            Product = await _context.Products.FindAsync(productId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order.Amount = Order.UnitCost * Order.Quantity;

            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
