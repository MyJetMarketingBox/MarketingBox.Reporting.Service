using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Grpc.Models
{
    [DataContract]
    public enum CustomersReportType
    {
        [DataMember(Order = 1)] Leads,
        [DataMember(Order = 2)] Deposits,
        [DataMember(Order = 3)] LeadsAndDeposits
    }
}