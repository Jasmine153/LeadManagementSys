using LeadManagementSys.Data;
using LeadManagementSys.Models.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Leads
{
    public class GetLeadByIdHandler : IRequestHandler<GetLeadByIdQuery, LeadUpdateRequest?>
    {
        private readonly LeadDbContext _context;
        private readonly ILogger<GetLeadByIdHandler> _logger;

        public GetLeadByIdHandler(LeadDbContext context, ILogger<GetLeadByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<LeadUpdateRequest?> Handle(GetLeadByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching lead details for Lead ID");
            var lead = await _context.Leads
                .Include(l => l.AssignedTo)
                .Include(l => l.Remarks)
                .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

            if (lead == null)
            {
                _logger.LogWarning("Lead not found.");
                return null;
            }


            return new LeadUpdateRequest
            {
                Id = lead.Id,
                LeadName = lead.LeadName,
                AssignedToId = lead.AssignedToId,
                Status = lead.Status,
                ExistingRemarks = lead.Remarks.Select(r => $"{r.CreatedByRole}: {r.Remark} ({r.CreatedAt.ToShortDateString()})").ToList()
            };
        }
    }

}
