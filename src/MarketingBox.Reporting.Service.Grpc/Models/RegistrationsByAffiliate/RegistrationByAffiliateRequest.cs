using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Grpc.Models.RegistrationsByAffiliate
{
    [DataContract]
    public class RegistrationByAffiliateRequest
    {
        [DataMember(Order = 1)] public string RegistrationUid { get; set; }
        [DataMember(Order = 2)] public long AffiliateId { get; set; }
        [DataMember(Order = 3)] public string TenantId { get; set; }
    }
}