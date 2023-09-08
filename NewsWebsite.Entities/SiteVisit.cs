using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Entities
{
    public class SiteVisit
    {
        [Key]
        public string IpAddress { get; set; }
        public DateTime VisitDateTime { get; set; }

    }
}
