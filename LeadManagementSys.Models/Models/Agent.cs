using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Models.Models
{
    public class Agent: ApplicationUser
    {
        public int NumberOfLeadsAssigned { get; set; } = 0;
        public int MaxLeadLimit { get; set; } = 5;
        public string? ManagerId { get; set; } 
        public Manager? Manager { get; set; }
    }
}
