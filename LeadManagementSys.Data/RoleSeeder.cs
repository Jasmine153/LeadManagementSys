using LeadManagementSys.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Data
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "SuperAdmin", "Admin", "Agent", "Manager" };

            foreach (var role in roles)
            {
                 if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var superAdminEmail = "superadmin@lead.com";
            var existingSuperAdmin = await userManager.FindByEmailAsync(superAdminEmail);

            if (existingSuperAdmin == null)
            {
                
                var superAdmin = new ApplicationUser
                {
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    FullName = "Super Admin",
                    EmailConfirmed = true
                };

                var createSuperAdmin = await userManager.CreateAsync(superAdmin, "Super@123");

                if (createSuperAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdmin, "Admin");
                }
            }
            else
            {
               
                var isSuperAdminInRole = await userManager.IsInRoleAsync(existingSuperAdmin, "SuperAdmin");

                if (!isSuperAdminInRole)
                {
                    await userManager.AddToRoleAsync(existingSuperAdmin, "SuperAdmin");
                }
            }
        }
    }
}
