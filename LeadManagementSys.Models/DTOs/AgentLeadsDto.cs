using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Models.DTOs
{
    public class AgentLeadsDto
    {
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public List<LeadResponse> Leads { get; set; }
    }
}
