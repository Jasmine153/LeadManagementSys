using LeadManagementSys.Data;
using LeadManagementSys.Models.DTOs;
using LeadManagementSys.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Managers
{
    public class GetManagerDashboardHandler : IRequestHandler<GetManagerDashboard, ManagerDashboardDto>
    {
        private readonly LeadDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetManagerDashboardHandler> _logger;

        public GetManagerDashboardHandler(LeadDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ILogger<GetManagerDashboardHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ManagerDashboardDto> Handle(GetManagerDashboard request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User not found in session.");
            }

            _logger.LogInformation("Fetching manager dashboard");

            var manager = await _userManager.Users
               .OfType<Manager>()
               .Include(m => m.Agents)
               .FirstOrDefaultAsync(m => m.Id == userId, cancellationToken);

            if (manager == null)
            {
                throw new UnauthorizedAccessException("Manager not found.");
            }


            var agents = await _userManager.Users
                .OfType<Agent>()
                .Where(a => a.ManagerId == manager.Id)
                .ToListAsync(cancellationToken);

            var agentLeads = agents.Select(agent => new AgentLeadsDto
            {
                AgentId = agent.Id,
                AgentName = agent.FullName,
                Leads = _context.Leads
          .Where(l => l.AssignedToId == agent.Id)
          .Include(l => l.Remarks)
          .Select(l => new LeadResponse
          {
              Id = l.Id,
              LeadName = l.LeadName,
              AssignedToName = agent.FullName,
              Status = l.Status.ToString(),
              CreatedAt = l.CreatedAt,
              Remarks = l.Remarks.Select(r => r.Remark).ToList()
          })
          .ToList()
            }).ToList();

            _logger.LogInformation("Dashboard prepared for Manager");
            return new ManagerDashboardDto
            {
                TotalAgents = agentLeads.Count,
                Agents = agentLeads
            };
        }
    }
}
