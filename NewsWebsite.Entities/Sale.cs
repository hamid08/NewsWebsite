using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Entities
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        public long TotalFare { get; set; }
        public long DriverValue { get; set; }
        public long TransportationCompanyValue { get; set; }
        public long TransportationUnitValue { get; set; }
        public long SumTotal { get; set; }
        public string TerminalCaption { get; set; }
        public TripType TripType { get; set; }
        public int TerminalId { get; set; }

        public ICollection<SettlementDetail> settlementDetails { get; set; }

    }

    public enum TripType
    {
        Turn,
        Tour,
        CallSale

    }
}
