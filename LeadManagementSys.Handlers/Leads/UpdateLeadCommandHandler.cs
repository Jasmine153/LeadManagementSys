using LeadManagementSys.Data;
using LeadManagementSys.Models.Enums;
using MediatR;
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

        public UpdateLeadCommandHandler(LeadDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateLeadCommand request, CancellationToken cancellationToken)
        {
            var lead = await _context.Leads.FindAsync(request.LeadRequest.Id);
            if (lead == null)
            {
                return false;
            }

            lead.Status = Enum.Parse<LeadStatus>(request.LeadRequest.Status.ToString());
            lead.Remarks = request.LeadRequest.Remarks;
            lead.AssignedToId = request.LeadRequest.AssignedToId;

            _context.Leads.Update(lead);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

    }
}
