using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Models.DTOs
{
    public class AgentDashboardDto
    {
        public int TotalLeads { get; set; }
        public List<LeadResponse> Leads { get; set; }
    }
}
