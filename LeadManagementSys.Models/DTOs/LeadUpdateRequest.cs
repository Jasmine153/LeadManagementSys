using LeadManagementSys.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Models.DTOs
{
    public class LeadUpdateRequest
    {
        public int Id { get; set; }
        public string LeadName { get; set; } = string.Empty;
        [Required]
        public LeadStatus Status { get; set; }
        public string? Remarks { get; set; }
        public string? AssignedToId { get; set; }
    }
}
