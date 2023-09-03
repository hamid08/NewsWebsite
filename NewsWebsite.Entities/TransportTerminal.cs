using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Entities
{
    public class TransportTerminal
    {
        public int Id { get; set; }
        public string Caption { get; set; }

        public ICollection<Sale> Sales { get; set; }

    }
}
