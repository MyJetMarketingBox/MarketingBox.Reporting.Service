using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Domain.Models.Registrations
{
    [DataContract]
    public class RegistrationRouteInfo
    {
        [DataMember(Order = 1)]
        public long AffiliateId { get; set; }

        [DataMember(Order = 2)]
        public long CampaignId { get; set; }

        [DataMember(Order = 3)]
        public long BrandId { get; set; }

        [DataMember(Order = 4)]
        public long IntegrationId { get; set; }
    }
}


