using LeadManagementSys.Data;
using LeadManagementSys.Handlers.SuperAdmin;
using LeadManagementSys.Models.DTOs;
using LeadManagementSys.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LeadManagementSys.Features.SuperAdmin.Dashboard
{
    public class GetDashboardSummaryHandler : IRequestHandler<GetDashboardSummary, DashboardSummaryDto>
    {
        private readonly LeadDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<GetDashboardSummaryHandler> _logger;

        public GetDashboardSummaryHandler(LeadDbContext context,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<GetDashboardSummaryHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<DashboardSummaryDto> Handle(GetDashboardSummary request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching dashboard summary");
            var totalAdmins = (await _userManager.GetUsersInRoleAsync("Admin")).Count;
            var totalAgents = (await _userManager.GetUsersInRoleAsync("Agent")).Count;
            var totalManagers = (await _userManager.GetUsersInRoleAsync("Manager")).Count;

            var totalLeads = await _context.Leads.CountAsync(cancellationToken);
            var leads = await _context.Leads
            .Include(l => l.AssignedTo)
            .Include(l => l.Remarks)
            .Select(l => new LeadResponse
           {
                Id = l.Id,
                LeadName = l.LeadName,
                 AssignedToName = l.AssignedTo != null ? l.AssignedTo.FullName : "Unassigned",
                Status = l.Status.ToString(),
                Remarks = l.Remarks.Select(r => r.Remark).ToList(),
                CreatedAt = l.CreatedAt
            }  ).ToListAsync(cancellationToken);
            _logger.LogInformation("Fetched leads from the database.");
            var agents = await _userManager.GetUsersInRoleAsync("Agent");
            var agentList = agents.Select(a => new AgentResponse
            {
                Id = a.Id,
                Name = a.FullName,
                Email = a.Email,
                NumberOfLeadsAssigned = _context.Leads.Count(l => l.AssignedToId == a.Id)
            }).ToList();

            
            var managers = await _userManager.GetUsersInRoleAsync("Manager");
            var managerList = managers.Select(m => new ManagerResponse
            {
                Id = m.Id,
                Name = m.FullName,
                Email = m.Email
            }).ToList();

            var leadsByStatus = await _context.Leads
            .GroupBy(l => l.Status)
            .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
            .ToDictionaryAsync(g => g.Status, g => g.Count, cancellationToken);

            _logger.LogInformation(" fetched Leads by status");
            return new DashboardSummaryDto
            {
                TotalAdmins = totalAdmins,
                TotalAgents = totalAgents,
                TotalManagers = totalManagers,
                TotalLeads = totalLeads,
                Leads = leads,
                Agents = agentList,
                Managers = managerList,
                LeadsByStatus = leadsByStatus
            };
        }
    }
}
