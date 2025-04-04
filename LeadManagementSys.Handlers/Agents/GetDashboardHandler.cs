using LeadManagementSys.Data;
using LeadManagementSys.Handlers.SuperAdmin;
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

namespace LeadManagementSys.Handlers.Agents
{

        public class GetDashboardHandler : IRequestHandler<GetDashboard, AgentDashboardDto>
        {
            private readonly LeadDbContext _context;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetDashboardHandler> _logger;

        public GetDashboardHandler(LeadDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ILogger<GetDashboardHandler> logger)
            {
                _context = context;
                _userManager = userManager;
                _httpContextAccessor = httpContextAccessor;
                _logger = logger;
        }

        public async Task<AgentDashboardDto> Handle(GetDashboard request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching agent dashboard data");
            var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User not found in session.");
            }

            var currentUser = await _userManager.FindByIdAsync(userId);
            if (currentUser == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }
            Console.WriteLine($"Fetching leads for user: {currentUser.Email} (ID: {userId})");
            var leads = await _context.Leads
                .Where(l => l.AssignedToId.ToString() == userId)
                .Include(l => l.AssignedTo)
                .Select(l => new LeadResponse
                {
                    Id = l.Id,
                    LeadName = l.LeadName,
                    AssignedToName = l.AssignedTo != null ? l.AssignedTo.FullName : "Unassigned",
                    Status = l.Status.ToString(),
                    Remarks = l.Remarks.Select(r => r.Remark).ToList(),
                    CreatedAt = l.CreatedAt
                })
                .ToListAsync(cancellationToken);
            _logger.LogInformation("Total leads found for agent {AgentEmail}: {LeadCount}", currentUser.Email, leads.Count);

            return new AgentDashboardDto
            {
                TotalLeads = leads.Count,
                Leads = leads
            };
        }

    }

}

