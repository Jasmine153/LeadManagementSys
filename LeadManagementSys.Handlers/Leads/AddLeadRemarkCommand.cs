using LeadManagementSys.Models.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Leads
{
    public class AddLeadRemarkCommand : IRequest<bool>
    {
        public AddLeadRemarkDto LeadRemarkRequest { get; set; }

        public AddLeadRemarkCommand(AddLeadRemarkDto leadRemarkRequest)
        {
            LeadRemarkRequest = leadRemarkRequest;
        }
    }
}
