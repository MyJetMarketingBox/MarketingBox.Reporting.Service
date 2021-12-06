using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Grpc.Models
{
    [DataContract]
    public class GetCustomerReportRequest
    {
        [DataMember(Order = 1)] public string UId { get; set; }
    }
}