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
        //public int TotalLeads { get; set; }
    }
}
