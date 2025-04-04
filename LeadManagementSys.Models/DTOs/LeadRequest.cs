using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Models.DTOs
{
    public class LeadRequest
    {
        [Required(ErrorMessage = "Lead Name is required.")]
        [StringLength(100, ErrorMessage = "Lead Name cannot exceed 100 characters.")]
        public string LeadName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select an agent to assign this lead.")]
        public string? AssignedToId { get; set; }

        [StringLength(500, ErrorMessage = "Remarks cannot exceed 500 characters.")]
        public string? Remarks { get; set; }
    }
}
