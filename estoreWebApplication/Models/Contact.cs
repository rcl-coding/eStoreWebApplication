#nullable disable
using System.ComponentModel.DataAnnotations;

namespace estoreWebApplication.Models
{
    public class Contact
    {
        [Required]
        [Display(Name = "Name")]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(250)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Message")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}
