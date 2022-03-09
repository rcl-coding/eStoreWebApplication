#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RCL.Azure.Storage.Core;
using Microsoft.AspNetCore.Authorization;
using estoreWebApplication.DataContext;
using estoreWebApplication.Models;

namespace estoreWebApplication.Areas.Admin.Pages.Products
{
    [Authorize(Policy = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly StoreDbContext _context;
        private readonly IAzureBlobStorageService _blob;

        public string ErrorMessage { get; set; } = string.Empty;

        [BindProperty]
        public Product Product { get; set; }

        public CreateModel(StoreDbContext context,
            IAzureBlobStorageService blob)
        {
            _context = context;
            _blob = blob;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (file == null)
            {
                ErrorMessage = "Please select an image file";
                return Page();
            }
            else
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
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            string fileExtension = FileHelper.GetFileExtension(file.FileName);
            string blobName = $"{Guid.NewGuid().ToString()}{fileExtension}";

            Product.ImageName = blobName;

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            using (var readStream = file.OpenReadStream())
            {
                var blob = await _blob.UploadBlobAsync(Helpers.Constants.AzureBlobStrorageContainer, ContainerType.Public, blobName, readStream, FileHelper.GetContentType(fileExtension));
            }

            return RedirectToPage("./Index");
        }
    }
}

