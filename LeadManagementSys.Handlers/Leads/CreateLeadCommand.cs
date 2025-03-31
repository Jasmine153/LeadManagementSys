using LeadManagementSys.Models.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Leads
{
    public class CreateLeadCommand : IRequest<bool>
    {
        public LeadRequest LeadRequest { get; set; }
        public string CreatedById { get; set; } = string.Empty;

        public CreateLeadCommand(LeadRequest leadRequest, string createdById)
        {
            LeadRequest = leadRequest;
            CreatedById = createdById;
        }
    }
}
