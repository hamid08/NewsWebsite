using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Entities
{
    public class SettlementDetail
    {
        public int Id { get; set; }

        public int SaleId { get; set; }

        public BenefisheryType BenefisheryType { get; set; }

        public int Value { get; set; }

        public Sale Sale { get; set; }


    }

    public enum BenefisheryType
    {
        Driver,
        Unit,
        Company
    }
}
