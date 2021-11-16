using System;

namespace MarketingBox.Reporting.Service.Postgres.ReadModels.Reports
{
    public class ReportEntity
    {
        public string TenantId { get; set; }
        public string UniqueId { get; set; }
        public long AffiliateId { get; set; }
        public long CampaignId { get; set; }
        public long BrandId { get; set; }
        public long IntegrationId { get; set; }
        public long RegistrationId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public decimal Payout { get; set; }
        public decimal Revenue { get; set; }
        public ReportType ReportType { get; set; }
    }
}
