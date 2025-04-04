using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Models.DTOs
{
    public class DashboardSummaryDto
    {
        public int TotalAdmins { get; set; }
        public int TotalAgents { get; set; }
        public int TotalManagers { get; set; }
        public int TotalLeads { get; set; }
        public Dictionary<string, int> LeadsByStatus { get; set; } = new Dictionary<string, int>();
        public List<LeadResponse> Leads { get; set; }
        public List<AgentResponse> Agents { get; set; } = new();
        public List<ManagerResponse> Managers { get; set; } = new();
    }
}
