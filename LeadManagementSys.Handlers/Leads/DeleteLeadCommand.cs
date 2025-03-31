using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Leads
{
    public class DeleteLeadCommand : IRequest<bool>
    {
        public int LeadId { get; set; }

        public DeleteLeadCommand(int leadId)
        {
            LeadId = leadId;
        }
    }
}
