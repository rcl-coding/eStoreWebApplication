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
    public class EditModel : PageModel
    {
        private readonly StoreDbContext _context;
        private readonly IAzureBlobStorageService _blob;

        public string ErrorMessage { get; set; } = string.Empty;

        [BindProperty]
        public Product Product { get; set; }

        public EditModel(StoreDbContext context,
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string oldImageName = string.Empty;
            string fileExtension = string.Empty;

            if (file != null)
            {
                if (file.Length < 1)
                {
                    ErrorMessage = "File error";
                    return Page();
                }

                if (FileHelper.IsImageFile(file.FileName) == false)
                {
                    ErrorMessage = "Only jpg, jpeg, png, gif, bmp, svg image files are allowed.";
                    return Page();
                }

                oldImageName = Product.ImageName;
                fileExtension = FileHelper.GetFileExtension(file.FileName);
                Product.ImageName = $"{Guid.NewGuid().ToString()}{fileExtension}";
            }

            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(oldImageName))
                {
                    using (var readStream = file.OpenReadStream())
                    {
                        var blob = await _blob.UploadBlobAsync(Helpers.Constants.AzureBlobStrorageContainer, ContainerType.Public, Product.ImageName, readStream, FileHelper.GetContentType(fileExtension));
                    }

                    int del = await _blob.DeleteBlobAsync(Helpers.Constants.AzureBlobStrorageContainer, oldImageName);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
