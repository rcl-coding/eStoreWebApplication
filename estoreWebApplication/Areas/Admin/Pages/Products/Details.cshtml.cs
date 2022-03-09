#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RCL.Azure.Storage.Core;
using Microsoft.AspNetCore.Authorization;
using estoreWebApplication.DataContext;
using estoreWebApplication.Models;

namespace estoreWebApplication.Areas.Admin.Pages.Products
{
    [Authorize(Policy = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly StoreDbContext _context;
        private readonly IAzureBlobStorageService _blob;

        public Product Product { get; set; }

        public DetailsModel(StoreDbContext context,
            IAzureBlobStorageService blob)
        {
            _context = context;
            _blob = blob;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(Product?.ImageName))
            {
                Product.ImageName = _blob.GetBlobSasUri(Helpers.Constants.AzureBlobStrorageContainer, Product.ImageName);
            }

            return Page();
        }
    }
}
