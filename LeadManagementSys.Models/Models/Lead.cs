using LeadManagementSys.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Models.Models
{
    public class Lead
    {
        public int Id { get; set; }
        public required string? LeadName { get; set; }

        [ForeignKey("AssignedTo")]
        public string? AssignedToId { get; set; }
        public ApplicationUser? AssignedTo { get; set; }
        public required LeadStatus Status { get; set; }
        public required string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
