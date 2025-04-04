// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using LeadManagementSys.Models.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace LeadManagementSys.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public List<SelectListItem> Managers { get; set; }
        public List<SelectListItem> Admins { get; set; }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string FullName { get; set; }
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }


            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; } = "Agent";

            [Display(Name = "Select Manager (if role is Agent)")]
            public string? SelectedManagerId { get; set; }

            [Display(Name = "Select Admin (if role is Manager)")]
            public string? SelectedAdminId { get; set; }


        }



        public async Task OnGetAsync(string returnUrl = null, string role = null)
        {
            ReturnUrl = returnUrl;
            Input = new InputModel
            {
                Role = role 
            };
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var managerUsers = await _userManager.GetUsersInRoleAsync("Manager");
            Managers = managerUsers.Any()
                ? managerUsers.Select(m => new SelectListItem { Value = m.Id, Text = m.FullName }).ToList()
                : new List<SelectListItem>();

            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
            Admins = adminUsers.Any()
                ? adminUsers.Select(a => new SelectListItem { Value = a.Id, Text = a.FullName }).ToList()
                : new List<SelectListItem>();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
{
    returnUrl ??= Url.Content("~/");
    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.FullName = Input.FullName;
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                user.Email = Input.Email;

                if (Input.Role == "Agent" && !string.IsNullOrEmpty(Input.SelectedManagerId))
                {
                    var agent = new LeadManagementSys.Models.Models.Agent
                    {
                        FullName = Input.FullName,
                        Email = Input.Email,
                        ManagerId = Input.SelectedManagerId,
                        UserName = Input.Email
                    };
                    user = agent;
                }
                else if (Input.Role == "Manager" && !string.IsNullOrEmpty(Input.SelectedAdminId))
                {
                    var manager = new LeadManagementSys.Models.Models.Manager
                    {
                        FullName = Input.FullName,
                        Email = Input.Email,
                        AdminId = Input.SelectedAdminId,
                        UserName = Input.Email
                    };
                    user = manager;
                }

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} created successfully with role {Role}.", user.Email, Input.Role);
                    TempData["success"] = "User created successfully!";


                    if (!await _roleManager.RoleExistsAsync(Input.Role))
                    {
                        var roleResult = await _roleManager.CreateAsync(new IdentityRole(Input.Role));
                        if (!roleResult.Succeeded)
                        {
                            _logger.LogError("Failed to create role {Role}.", Input.Role);
                            ModelState.AddModelError("", "Failed to create role.");
                            return Page();
                        }
                    }

           
                    var roleAssignmentResult = await _userManager.AddToRoleAsync(user, Input.Role);
                    if (roleAssignmentResult.Succeeded)
                    {
                        _logger.LogInformation($"Role {Input.Role} assigned to user {user.Email}");
                    }
                    else
                    {
                        _logger.LogError($"Failed to assign role {Input.Role} to user {user.Email}");
                    }

                    var currentUser = await _userManager.GetUserAsync(User);
                 
                    var userRoles = await _userManager.GetRolesAsync(currentUser);
                    if (userRoles.Contains("SuperAdmin"))
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }
                    else if (userRoles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }
                    else if (userRoles.Contains("Manager"))
                    {
                        return RedirectToAction("Index", "ManagerDashboard", new { area = "Manager" });
                    }
                    else if (userRoles.Contains("Agent"))
                    {
                        return RedirectToAction("Index", "AgentDashboard", new { area = "Agent" });
                    }

                    return RedirectToAction("Index", "Home");

                }

              
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

         
            return Page();
}


        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
