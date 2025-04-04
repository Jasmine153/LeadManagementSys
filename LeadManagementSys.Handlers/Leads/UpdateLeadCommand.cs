using LeadManagementSys.Models.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Leads
{
    public class UpdateLeadCommand : IRequest<bool>
    {
        public LeadUpdateRequest LeadRequest { get; set; }
        public string UpdatedById { get; set; }
        public string UpdatedByRole { get; set; }

        public UpdateLeadCommand(LeadUpdateRequest leadRequest, string updatedById, string updatedByRole)
        {
            LeadRequest = leadRequest;
            UpdatedById = updatedById;
            UpdatedByRole = updatedByRole;
        }
    }
}
