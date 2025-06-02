using Microsoft.AspNetCore.Identity;

namespace Uppari_SCMS.Data.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
