using System.Runtime.Serialization;
using MarketingBox.Reporting.Service.Domain.Models;

namespace MarketingBox.Reporting.Service.Grpc.Models
{
    [DataContract]
    public class GetCustomerReportResponse
    {
        [DataMember(Order = 1)] public bool Success { get; set; }
        [DataMember(Order = 2)] public string ErrorMessage { get; set; }
        [DataMember(Order = 3)] public Customer Customer { get; set; }
    }
}