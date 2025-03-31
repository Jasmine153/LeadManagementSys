using LeadManagementSys.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Leads
{
    public class DeleteLeadHandler : IRequestHandler<DeleteLeadCommand, bool>
    {
        private readonly LeadDbContext _context;

        public DeleteLeadHandler(LeadDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteLeadCommand request, CancellationToken cancellationToken)
        {
            var lead = await _context.Leads.FindAsync(request.LeadId);
            if (lead == null)
                return false;

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
