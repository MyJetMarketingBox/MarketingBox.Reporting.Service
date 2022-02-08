using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Grpc.Models.Reports
{
    [DataContract]
    public class Report
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }

        [DataMember(Order = 2)]
        public long RegistrationCount { get; set; }

        [DataMember(Order = 3)]
        public long FtdCount { get; set; }

        [DataMember(Order = 4)]
        public decimal Payout { get; set; }

        [DataMember(Order = 5)]
        public decimal Revenue { get; set; }

        [DataMember(Order = 6)]
        public decimal Cr { get; set; }
    }
}
