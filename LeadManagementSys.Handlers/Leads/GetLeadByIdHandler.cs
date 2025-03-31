using LeadManagementSys.Data;
using LeadManagementSys.Models.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Leads
{
    public class GetLeadByIdHandler : IRequestHandler<GetLeadByIdQuery, LeadUpdateRequest>
    {
        private readonly LeadDbContext _context;

        public GetLeadByIdHandler(LeadDbContext context)
        {
            _context = context;
        }

        public async Task<LeadUpdateRequest?> Handle(GetLeadByIdQuery request, CancellationToken cancellationToken)
        {
            var lead = await _context.Leads
                .Include(l => l.AssignedTo)
                .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

            if (lead == null)
                return null;

            return new LeadUpdateRequest
            {
                Id = lead.Id,
                LeadName = lead.LeadName,
                AssignedToId = lead.AssignedToId,
                Status = lead.Status.ToString(),
                Remarks = lead.Remarks
            };
        }
    }
}
