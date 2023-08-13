using NewsWebsite.Entities.identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Entities
{
    public class UserCategory
    {
        public int UserId { get; set; }

        public string CategoryId { get; set; }

        public virtual User User { get; set; }
        public virtual Category Category { get; set; }

    }
}
