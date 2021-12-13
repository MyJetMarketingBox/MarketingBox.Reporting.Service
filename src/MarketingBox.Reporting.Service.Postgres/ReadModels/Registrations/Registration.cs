using System;
using MarketingBox.Reporting.Service.Domain.Lead;

namespace MarketingBox.Reporting.Service.Postgres.ReadModels.Registrations
{
    public class Registration
    {
        public string TenantId { get; set; }
        public string UniqueId { get; set; }
        public long RegistrationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Ip { get; set; }
        public long AffiliateId { get; set; }
        public long BrandId { get; set; }
        public long CampaignId { get; set; }
        public long IntegrationId { get; set; }
        public LeadStatus Status { get; set; }
        public LeadCrmStatus CrmStatus{ get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public long Sequence { get; set; }
        public string So { get; set; }
        public string Sub { get; set; }
        public string Sub1 { get; set; }
        public string Sub2 { get; set; }
        public string Sub3 { get; set; }
        public string Sub4 { get; set; }
        public string Sub5 { get; set; }
        public string Sub6 { get; set; }
        public string Sub7 { get; set; }
        public string Sub8 { get; set; }
        public string Sub9 { get; set; }
        public string Sub10 { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset? ConversionDate { get; set; }
        public DateTimeOffset? DepositDate { get; set; }
        public string Country { get; set; }

        //public IList<AffiliateAccess> AffiliateAccesses { get; set; }
    }
}
