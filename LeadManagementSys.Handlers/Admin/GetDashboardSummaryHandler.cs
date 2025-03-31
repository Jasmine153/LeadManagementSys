using LeadManagementSys.Data;
using LeadManagementSys.Handlers.SuperAdmin;
using LeadManagementSys.Models.DTOs;
using LeadManagementSys.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementSys.Features.SuperAdmin.Dashboard
{
    public class GetDashboardSummaryHandler : IRequestHandler<GetDashboardSummary, DashboardSummaryDto>
    {
        private readonly LeadDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetDashboardSummaryHandler(LeadDbContext context,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<DashboardSummaryDto> Handle(GetDashboardSummary request, CancellationToken cancellationToken)
        {
            var totalAdmins = (await _userManager.GetUsersInRoleAsync("Admin")).Count;
            var totalAgents = (await _userManager.GetUsersInRoleAsync("Agent")).Count;
            var totalManagers = (await _userManager.GetUsersInRoleAsync("Manager")).Count;

            var totalLeads = await _context.Leads.CountAsync(cancellationToken);
            var leads = await _context.Leads
               .Include(l => l.AssignedTo)
               .Select(l => new LeadResponse
               {
                   Id = l.Id,
                   LeadName = l.LeadName,
                   AssignedToName = l.AssignedTo != null ? l.AssignedTo.FullName : "Unassigned",
                   Status = l.Status.ToString(),
                   Remarks = l.Remarks,
                   CreatedAt = l.CreatedAt
               }).ToListAsync(cancellationToken);

            return new DashboardSummaryDto
            {
                TotalAdmins = totalAdmins,
                TotalAgents = totalAgents,
                TotalManagers = totalManagers,
                TotalLeads = totalLeads,
                Leads = leads
            };
        }
    }
}
