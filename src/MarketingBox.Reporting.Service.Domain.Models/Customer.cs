using System;
using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Domain.Models
{
    [DataContract]
    public class Customer
    {
        [DataMember(Order = 1)] public long Id { get; set; }
        [DataMember(Order = 2)] public string UId { get; set; }
        
        [DataMember(Order = 3)] public DateTime CreatedDate { get; set; }
        [DataMember(Order = 4)] public string TenantId { get; set; }
        
        [DataMember(Order = 5)] public string FirstName { get; set; }
        [DataMember(Order = 6)] public string LastName { get; set; }
        [DataMember(Order = 7)] public string Email { get; set; }
        [DataMember(Order = 8)] public string Phone { get; set; }
        [DataMember(Order = 9)] public string Ip { get; set; }
        [DataMember(Order = 10)] public string Country { get; set; }
        
        [DataMember(Order = 11)] public long AffiliateId { get; set; }
        [DataMember(Order = 12)] public long BrandId { get; set; }
        [DataMember(Order = 13)] public long CampaignId { get; set; }
        
        [DataMember(Order = 14)] public bool IsDeposit { get; set; }
        [DataMember(Order = 15)] public DateTime DepositDate { get; set; }
    }
}