using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Models.DTOs
{

    public class ManagerDashboardDto
    {
        public int TotalAgents { get; set; }
        public List<AgentLeadsDto> Agents { get; set; }
    }


}
