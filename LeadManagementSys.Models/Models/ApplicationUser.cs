using Microsoft.AspNetCore.Identity;

namespace LeadManagementSys.Models.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

    }
}
