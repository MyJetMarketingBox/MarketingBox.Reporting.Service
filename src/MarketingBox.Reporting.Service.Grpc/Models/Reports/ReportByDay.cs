using System;
using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Grpc.Models.Reports
{
    [DataContract]
    public class ReportByDay
    {
        [DataMember(Order = 1)]
        public DateTime CreatedAt { get; set; }

        [DataMember(Order = 2)]
        public long RegistrationCount { get; set; }

        [DataMember(Order = 3)]
        public long FtdCount { get; set; }
    }
}
