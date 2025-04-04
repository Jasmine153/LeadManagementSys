using LeadManagementSys.Data;
using LeadManagementSys.Models.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Leads
{
    public class GetAllLeadsHandler : IRequestHandler<GetAllLeadsQuery, List<LeadResponse>>
    {
        private readonly LeadDbContext _context;

        public GetAllLeadsHandler(LeadDbContext context)
        {
            _context = context;
        }

        public async Task<List<LeadResponse>> Handle(GetAllLeadsQuery request, CancellationToken cancellationToken)
        {
            var leads = await _context.Leads
                .Include(l => l.AssignedTo)
                .Include(l => l.Remarks)
                .ToListAsync(cancellationToken);

            return leads.Select(l => new LeadResponse
            {
                Id = l.Id,
                LeadName = l.LeadName,
                AssignedToName = l.AssignedTo != null ? l.AssignedTo.FullName : "Unassigned",
                Status = l.Status.ToString(),
                Remarks = l.Remarks.OrderByDescending(r => r.CreatedAt).Select(r => r.Remark).ToList(),
                CreatedAt = l.CreatedAt
            }).ToList();
        }
    }
}
