using LeadManagementSys.Models.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Leads
{
      public class GetLeadByIdQuery : IRequest<LeadUpdateRequest>
    {
        public int Id { get; set; }

        public GetLeadByIdQuery(int id)
        {
            Id = id;
        }
    }
}
