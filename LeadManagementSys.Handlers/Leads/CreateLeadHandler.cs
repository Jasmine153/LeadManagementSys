using LeadManagementSys.Data;
using LeadManagementSys.Models.DTOs;
using LeadManagementSys.Models.Enums;
using LeadManagementSys.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Leads
{
    public class CreateLeadHandler : IRequestHandler<CreateLeadCommand, bool>
    {
        private readonly LeadDbContext _context;

        public CreateLeadHandler(LeadDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateLeadCommand request, CancellationToken cancellationToken)
        {
            var lead = new Lead
            {
                LeadName = request.LeadRequest.LeadName,
                AssignedToId = request.LeadRequest.AssignedToId ?? request.CreatedById,
                Status = LeadStatus.Pending,
                Remarks = request.LeadRequest.Remarks,
                CreatedAt = DateTime.Now
            };

            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
