﻿using System;
using System.Collections.Generic;
using MarketingBox.Reporting.Service.Domain.Crm;
using MarketingBox.Reporting.Service.Domain.Deposit;
using MarketingBox.Reporting.Service.Postgres.ReadModels.AffiliateAccesses;

namespace MarketingBox.Reporting.Service.Postgres.ReadModels.Deposits
{
    public class Deposit
    {
        public string TenantId { get; set; }
        public string UniqueId { get; set; }
        public string CustomerId { get; set; }
        public string Country { get; set; }
        public long RegistrationId { get; set; }
        public string Email { get; set; }
        public long AffiliateId { get; set; }
        public long BrandId { get; set; }
        public long CampaignId { get; set; }
        public long IntegrationId { get; set; }
        public ApprovedType Type { get; set; }
        public DateTimeOffset RegisterDate { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ConversionDate { get; set; }
        public long Sequence { get; set; }
        public CrmStatus CrmStatus { get; set; }

        //public IList<AffiliateAccess> AffiliateAccesses { get; set; }
    }
}
