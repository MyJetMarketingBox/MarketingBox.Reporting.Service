using System;
using System.Runtime.Serialization;
using MarketingBox.Sdk.Common.Enums;

namespace MarketingBox.Reporting.Service.Grpc.Requests.Registrations
{
    [DataContract]
    public class RegistrationSearchRequest
    {
        [DataMember(Order = 1)] public long? AffiliateId { get; set; }
        [DataMember(Order = 2)] public long? Cursor { get; set; }
        [DataMember(Order = 3)] public int? Take { get; set; }
        [DataMember(Order = 4)] public bool Asc { get; set; }
        [DataMember(Order = 5)] public string? TenantId { get; set; }
        [DataMember(Order = 6)] public RegistrationsReportType? Type { get; set; }
        [DataMember(Order = 7)] public string? Country { get; set; }
        [DataMember(Order = 8)] public RegistrationStatus? Status { get; set; }
        [DataMember(Order = 9)] public CrmStatus? CrmStatus { get; set; }
        [DataMember(Order = 10)] public DateTime? DateFrom { get; set; }
        [DataMember(Order = 11)] public DateTime? DateTo { get; set; }
    }
}