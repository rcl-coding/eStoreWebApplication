using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace estoreWebApplication.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        public Models.Contact Contact { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // TODO - send email

            return RedirectToPage("MessageSent");
        }
    }
}
