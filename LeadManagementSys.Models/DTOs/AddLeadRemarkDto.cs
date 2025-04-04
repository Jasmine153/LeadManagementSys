using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Models.DTOs
{
    public class AddLeadRemarkDto
    {
        public int LeadId { get; set; }
        public string Remark { get; set; } = null!;
        public string CreatedById { get; set; } = null!;
    }
}
