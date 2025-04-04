using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Models.DTOs
{
    public class LeadResponse
    {
        public int Id { get; set; }
        public string LeadName { get; set; } = string.Empty;
        public string? AssignedToName { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<string> Remarks { get; set; } = new List<string>();

    }
}
