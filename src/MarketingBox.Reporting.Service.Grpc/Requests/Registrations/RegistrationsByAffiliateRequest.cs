using System;
using System.Runtime.Serialization;
using MarketingBox.Sdk.Common.Enums;

namespace MarketingBox.Reporting.Service.Grpc.Requests.Registrations
{
    [DataContract]
    public class RegistrationsByAffiliateRequest
    {
        [DataMember(Order = 1)] public DateTime From { get; set; }
        [DataMember(Order = 2)] public DateTime To { get; set; }
        [DataMember(Order = 3)] public RegistrationsReportType Type { get; set; }
        [DataMember(Order = 4)] public long AffiliateId { get; set; }
        [DataMember(Order = 10)] public long? Cursor { get; set; }
        [DataMember(Order = 11)] public int? Take { get; set; }
        [DataMember(Order = 12)] public bool Asc { get; set; }
        [DataMember(Order = 13)] public string TenantId { get; set; }

    }
}