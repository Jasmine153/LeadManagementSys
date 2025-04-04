using LeadManagementSys.Data;
using LeadManagementSys.Models.Enums;
using LeadManagementSys.Models.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Leads
{
    public class UpdateLeadCommandHandler : IRequestHandler<UpdateLeadCommand, bool>
    {
        private readonly LeadDbContext _context;
        private readonly ILogger<UpdateLeadCommandHandler> _logger;

        public UpdateLeadCommandHandler(LeadDbContext context, ILogger<UpdateLeadCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateLeadCommand request, CancellationToken cancellationToken)
        {
            var lead = await _context.Leads
                .Include(l => l.Remarks)
                .FirstOrDefaultAsync(l => l.Id == request.LeadRequest.Id, cancellationToken);

            if (lead == null)
            {
                _logger.LogWarning("Lead not found.");
                return false;
            }

            if (!string.IsNullOrEmpty(request.LeadRequest.LeadName))
            {
                lead.LeadName = request.LeadRequest.LeadName;
            }

            if (!string.IsNullOrEmpty(request.LeadRequest.AssignedToId))
            {
                lead.AssignedToId = request.LeadRequest.AssignedToId;
            }

            if (!string.IsNullOrEmpty(request.LeadRequest.Status.ToString()))
            {
                lead.Status = request.LeadRequest.Status;
            }

            if (!string.IsNullOrWhiteSpace(request.LeadRequest.Remarks))
            {
                var leadRemark = new LeadRemark
                {
                    LeadId = lead.Id,
                    Remark = request.LeadRequest.Remarks,
                    CreatedById = request.UpdatedById,
                    CreatedAt = DateTime.UtcNow,
                    CreatedByRole = request.UpdatedByRole
                };

                _context.LeadRemarks.Add(leadRemark);
            }

            _context.Leads.Update(lead);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Lead updated Successfully ");
            return true;
        }
    }


}
