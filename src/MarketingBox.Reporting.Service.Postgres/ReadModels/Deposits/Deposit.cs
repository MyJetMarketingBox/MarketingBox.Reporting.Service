﻿using System;
using MarketingBox.Reporting.Service.Domain.Deposit;

namespace MarketingBox.Reporting.Service.Postgres.ReadModels.Deposits
{
    public class Deposit
    {
        public string TenantId { get; set; }
        public string UniqueId { get; set; }
        public string CustomerId { get; set; }
        public string Country { get; set; }
        public long LeadId { get; set; }
        public string Email { get; set; }
        public long AffiliateId { get; set; }
        public long CampaignId { get; set; }
        public long BoxId { get; set; }
        public long BrandId { get; set; }
        public ApprovedType Type { get; set; }
        public DateTimeOffset RegisterDate { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ConversionDate { get; set; }
        public long Sequence { get; set; }
        public string BrandStatus { get; set; }
    }
}
