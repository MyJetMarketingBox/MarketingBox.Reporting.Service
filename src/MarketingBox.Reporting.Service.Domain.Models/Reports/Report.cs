using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Domain.Models.Reports
{
    [DataContract]
    public class Report
    {
        [DataMember(Order = 1)]
        public long Id { get; set; }
        
        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public int RegistrationCount { get; set; }

        [DataMember(Order = 4)]
        public int FtdCount { get; set; }

        [DataMember(Order = 5)]
        public decimal Payout { get; set; }

        [DataMember(Order = 6)]
        public decimal Revenue { get; set; }

        [DataMember(Order = 7)]
        public decimal? Cr { get; set; }
        
        [DataMember(Order = 8)]
        public decimal Pl { get; set; }
        
        [DataMember(Order = 9)]
        public decimal? Epc { get; set; }
        
        [DataMember(Order = 10)]
        public decimal? Epl { get; set; }
        
        [DataMember(Order = 11)]
        public decimal? Roi { get; set; }
        
        [DataMember(Order = 12)]
        public decimal? Clicks { get; set; }
        
        [DataMember(Order = 13)]
        public int FailedCount { get; set; }
        
        [DataMember(Order = 14)]
        public int UnassignedCount { get; set; }
    }
}
