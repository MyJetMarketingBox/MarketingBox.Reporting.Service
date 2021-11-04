using System.Collections.Generic;
using System.Runtime.Serialization;
using MarketingBox.Reporting.Service.Grpc.Models.Common;

namespace MarketingBox.Reporting.Service.Grpc.Models.Reports
{
    [DataContract]
    public class ReportByDaySearchResponse
    {
        [DataMember(Order = 1)]
        public IReadOnlyCollection<ReportByDay> Reports { get; set; }

        [DataMember(Order = 100)]
        public Error Error { get; set; }
    }
}