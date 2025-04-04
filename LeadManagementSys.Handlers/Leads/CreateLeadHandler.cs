using LeadManagementSys.Data;
using LeadManagementSys.Models.DTOs;
using LeadManagementSys.Models.Enums;
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
    public class CreateLeadHandler : IRequestHandler<CreateLeadCommand, bool>
    {
        private readonly LeadDbContext _context;
        private readonly ILogger<CreateLeadHandler> _logger;

        public CreateLeadHandler(LeadDbContext context, ILogger<CreateLeadHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateLeadCommand request, CancellationToken cancellationToken)
        {
            var lead = new Lead
            {
                LeadName = request.LeadRequest.LeadName,
                AssignedToId = request.LeadRequest.AssignedToId ?? request.CreatedById,
                Status = LeadStatus.Assigned,
                CreatedAt = DateTime.UtcNow
            };

            _context.Leads.Add(lead);
            await _context.SaveChangesAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.LeadRequest.Remarks))
            {
                var leadRemark = new LeadRemark
                {
                    LeadId = lead.Id,
                    Remark = request.LeadRequest.Remarks,
                    CreatedById = request.CreatedById,
                    CreatedAt = DateTime.UtcNow
                };

                _context.LeadRemarks.Add(leadRemark);
                await _context.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation("Lead {LeadId} created successfully.", lead.Id);
            return true;
        }
    }

}
