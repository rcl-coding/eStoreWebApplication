#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using estoreWebApplication.Models;
using RCL.Azure.Storage.Core;
using Microsoft.AspNetCore.Authorization;
using estoreWebApplication.DataContext;
using estoreWebApplication.Helpers;

namespace estoreWebApplication.Areas.Admin.Pages.Products
{
    [Authorize(Policy = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly StoreDbContext _context;
        private readonly IAzureBlobStorageService _blob;

        public IList<Product> Products { get; set; } = new List<Product>();

        public IndexModel(StoreDbContext context,
            IAzureBlobStorageService blob)
        {
            _context = context;
            _blob = blob;
        }

        public async Task OnGetAsync()
        {
            List<Product> _products = await _context.Products.ToListAsync();

            if (_products?.Count > 0)
            {
                foreach (var product in _products)
                {
                    if (!string.IsNullOrEmpty(product.ImageName))
                    {
                        product.ImageName = _blob.GetBlobSasUri(Constants.AzureBlobStrorageContainer, product.ImageName);
                    }
                }

                Products = _products.OrderBy(o => o.SortCode).ToList();
            }
        }
    }
}

