using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Models.DTOs
{
    public class LeadRequest
    {
        public string LeadName { get; set; } = string.Empty;
        public string? AssignedToId { get; set; } 
        public string? Remarks { get; set; }
    }
}
