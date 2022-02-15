using System.Collections.Generic;
using System.Runtime.Serialization;
using MarketingBox.Reporting.Service.Domain.Models.Reports;
using MarketingBox.Reporting.Service.Grpc.Models.Common;

namespace MarketingBox.Reporting.Service.Grpc.Models.Reports
{
    [DataContract]
    public class ReportSearchResponse
    {
        [DataMember(Order = 1)]
        public IReadOnlyCollection<Report> Reports { get; set; }

        [DataMember(Order = 100)]
        public Error Error { get; set; }
    }
}