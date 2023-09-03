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
     
        public TripType TripType { get; set; }
        public int TransportTerminalId { get; set; }

        public TransportTerminal TransportTerminal { get; set; }

        public TripStatus TripStatus { get; set; }

        public DateTime? EndMissionDate { get; set; }


        public ICollection<SettlementDetail> settlementDetails { get; set; }

    }

    public enum TripStatus
    {
       EndMission,
       Started,
       InMission

    }

    public enum TripType
    {
        Turn,
        Tour,
        CallSale

    }
}
