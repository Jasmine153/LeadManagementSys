using LeadManagementSys.Data;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<DeleteLeadHandler> _logger;

        public DeleteLeadHandler(LeadDbContext context, ILogger<DeleteLeadHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteLeadCommand request, CancellationToken cancellationToken)
        {
            var lead = await _context.Leads.FindAsync(request.LeadId);
            if (lead == null)
                return false;

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Lead deleted successfully.");
            return true;
        }
    }
}
