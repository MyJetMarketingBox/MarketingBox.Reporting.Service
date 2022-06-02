using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Domain.Models.TrackingLinks;

[DataContract]
public class TrackingLink
{
    [DataMember(Order = 1)] public long Id { get; set; }
    [DataMember(Order = 2)] public long ClickId { get; set; }
    [DataMember(Order = 3)] public long BrandId { get; set; }
    [DataMember(Order = 4)] public long AffiliateId { get; set; }
    [DataMember(Order = 5)] public string Link { get; set; }
    [DataMember(Order = 6)] public LinkParameterValues LinkParameterValues { get; set; }
    [DataMember(Order = 7)] public LinkParameterNames LinkParameterNames { get; set; }
    [DataMember(Order = 8)] public string UniqueId { get; set; }
    
    [DataMember(Order = 9)] public long? RegistrationId { get; set; }
    [DataMember(Order = 10)] public string TenantId { get; set; }
    [DataMember(Order = 11)] public long? OfferId { get; set; }
}