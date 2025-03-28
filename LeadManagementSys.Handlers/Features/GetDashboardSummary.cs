using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadManagementSys.Models.DTOs;

namespace LeadManagementSys.Handlers.Features
{
    public class GetDashboardSummary : IRequest<DashboardSummaryDto>
    {
    }
}
