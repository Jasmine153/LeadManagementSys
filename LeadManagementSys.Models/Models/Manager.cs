using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Models.Models
{
    public class Manager: ApplicationUser
    {
        public string? AdminId { get; set; } // Reference to Admin
        public Admin? Admin { get; set; }
        public ICollection<Agent>? Agents { get; set; }
    }
}
