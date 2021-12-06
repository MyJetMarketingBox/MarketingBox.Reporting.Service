using System;
using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Grpc.Models
{
    [DataContract]
    public class GetCustomersReportRequest
    {
        [DataMember(Order = 1)] public DateTime From { get; set; }
        [DataMember(Order = 2)] public DateTime To { get; set; }
        [DataMember(Order = 3)] public CustomersReportType Type { get; set; }
    }
}