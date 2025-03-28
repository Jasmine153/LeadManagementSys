using LeadManagementSys.Handlers.Features;
using LeadManagementSys.Models.DTOs;
using LeadManagementSys.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementSys.Features.SuperAdmin.Dashboard
{
    public class GetDashboardSummaryHandler : IRequestHandler<GetDashboardSummary, DashboardSummaryDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetDashboardSummaryHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<DashboardSummaryDto> Handle(GetDashboardSummary request, CancellationToken cancellationToken)
        {
            var totalAdmins = (await _userManager.GetUsersInRoleAsync("Admin")).Count;
            var totalAgents = (await _userManager.GetUsersInRoleAsync("Agent")).Count;
            var totalManagers = (await _userManager.GetUsersInRoleAsync("Manager")).Count;

            // Assuming Leads are in ApplicationDbContext
            //var totalLeads = await _userManager.Users
            //    .Where(u => u.Leads != null)
            //    .SelectMany(u => u.Leads)
            //    .CountAsync(cancellationToken);

            return new DashboardSummaryDto
            {
                TotalAdmins = totalAdmins,
                TotalAgents = totalAgents,
                TotalManagers = totalManagers,
                //TotalLeads = totalLeads
            };
        }
    }
}
