using Microsoft.AspNetCore.Identity;

namespace mvc_shop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = default!;
    }
}

