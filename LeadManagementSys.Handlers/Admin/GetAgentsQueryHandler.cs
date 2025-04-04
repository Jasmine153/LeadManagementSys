using LeadManagementSys.Data;
using LeadManagementSys.Models.DTOs;
using LeadManagementSys.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Admin
{
    public class GetAgentsQueryHandler : IRequestHandler<GetAgentsQuery, List<AgentResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LeadDbContext _context;
        private readonly ILogger<GetAgentsQueryHandler> _logger;
        public GetAgentsQueryHandler(UserManager<ApplicationUser> userManager, LeadDbContext context, ILogger<GetAgentsQueryHandler> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task<List<AgentResponse>> Handle(GetAgentsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching agents with assigned leads");
            var agents = await _context.Users
          .OfType<Agent>()
          .Include(a => a.Manager)
          .ToListAsync(cancellationToken);

            _logger.LogInformation("Fetched agents from the database.");
            return agents.Select(a => new AgentResponse
            {
                Id = a.Id,
                Name = a.FullName,
                Email = a.Email,
                NumberOfLeadsAssigned = _context.Leads.Count(l => l.AssignedToId == a.Id),
                ManagerName = a.Manager != null ? a.Manager.FullName : "Not Assigned"
            }).ToList();
        }
    }
}
