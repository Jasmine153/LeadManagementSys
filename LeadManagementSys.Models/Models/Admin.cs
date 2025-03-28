using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Models.Models
{
    public class Admin : ApplicationUser
    {
        public ICollection<Manager>? Managers { get; set; }
    }
}
