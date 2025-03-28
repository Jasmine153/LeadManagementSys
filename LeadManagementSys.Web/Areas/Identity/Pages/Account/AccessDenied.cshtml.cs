using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LeadManagementSys.Areas.Identity.Pages.Account // ✅ Make sure namespace is correct
{
    public class AccessDeniedModel : PageModel // ✅ Class name should match
    {
        public void OnGet()
        {
        }
    }
}
