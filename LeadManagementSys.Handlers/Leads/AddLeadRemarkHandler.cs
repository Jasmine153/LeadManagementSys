using LeadManagementSys.Data;
using LeadManagementSys.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Leads
{
    public class AddLeadRemarkHandler : IRequestHandler<AddLeadRemarkCommand, bool>
    {
        private readonly LeadDbContext _context;
        private readonly ILogger<AddLeadRemarkHandler> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddLeadRemarkHandler(LeadDbContext context, ILogger<AddLeadRemarkHandler> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<bool> Handle(AddLeadRemarkCommand request, CancellationToken cancellationToken)
        {
            var lead = await _context.Leads.FindAsync(request.LeadRemarkRequest.LeadId);
            if (lead == null)
            {
                _logger.LogWarning("Lead {LeadId} not found", request.LeadRemarkRequest.LeadId);
                return false;
            }

            var user = await _userManager.FindByIdAsync(request.LeadRemarkRequest.CreatedById);
            var userRoles = await _userManager.GetRolesAsync(user);
            string userRole = userRoles.FirstOrDefault() ?? "Unknown";

            var remark = new LeadRemark
            {
                LeadId = request.LeadRemarkRequest.LeadId,
                Remark = request.LeadRemarkRequest.Remark,
                CreatedById = request.LeadRemarkRequest.CreatedById,
                CreatedByRole = userRole,
                CreatedAt = DateTime.UtcNow
            };

            _context.LeadRemarks.Add(remark);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Remark added to lead {LeadId}: {RemarkText}", request.LeadRemarkRequest.LeadId, request.LeadRemarkRequest.Remark);
            return true;
        }
    }
}
