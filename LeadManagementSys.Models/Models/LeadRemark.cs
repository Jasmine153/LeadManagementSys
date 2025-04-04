using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Models.Models
{
    public class LeadRemark
    {
        public int Id { get; set; }

        [Required]
        public string Remark { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string CreatedById { get; set; } = null!;

        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { get; set; } = null!;

        [Required]
        public int LeadId { get; set; }

        [ForeignKey("LeadId")]
        public Lead Lead { get; set; } = null!;

        [Required]
        public string CreatedByRole { get; set; } = "Agent";
    }
}
