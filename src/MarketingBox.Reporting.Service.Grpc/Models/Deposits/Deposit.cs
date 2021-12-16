using System.Runtime.Serialization;
using MarketingBox.Reporting.Service.Domain.Models.Deposit;
using System;
using MarketingBox.Reporting.Service.Domain.Crm;

namespace MarketingBox.Reporting.Service.Grpc.Models.Deposits
{
    [DataContract]
    public class Deposit
    {
        [DataMember(Order = 1)]
        public string TenantId { get; set; }
        [DataMember(Order = 2)]
        public string UniqueId { get; set; }
        [DataMember(Order = 3)]
        public string CustomerId { get; set; }
        [DataMember(Order = 4)]
        public string Country { get; set; }
        [DataMember(Order = 5)]
        public long LeadId { get; set; }
        [DataMember(Order = 6)]
        public string Email { get; set; }
        [DataMember(Order = 7)]
        public long AffiliateId { get; set; }
        [DataMember(Order = 8)]
        public long CampaignId { get; set; }
        [DataMember(Order = 9)]
        public long BoxId { get; set; }
        [DataMember(Order = 10)]
        public long BrandId { get; set; }
        [DataMember(Order = 11)]
        public ApprovedType Type { get; set; }
        [DataMember(Order = 12)]
        public DateTime RegisterDate { get; set; }
        [DataMember(Order = 13)]
        public DateTime CreatedAt { get; set; }
        [DataMember(Order = 14)]
        public DateTime? ConversionDate { get; set; }
        [DataMember(Order = 15)]
        public long Sequence { get; set; }

        [DataMember(Order = 16)]
        public CrmStatus CrmStatus { get; set; }
        
        [DataMember(Order = 17)]
        public long RegistrationId { get; set; }
    }
}
