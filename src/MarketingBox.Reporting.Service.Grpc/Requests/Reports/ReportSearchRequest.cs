using System;
using System.Runtime.Serialization;
using MarketingBox.Sdk.Common.Enums;
using MarketingBox.Sdk.Common.Models;

namespace MarketingBox.Reporting.Service.Grpc.Requests.Reports
{
    [DataContract]
    public class ReportSearchRequest : ValidatableEntity
    {
        [DataMember(Order = 1)] public long? AffiliateId { get; set; }
        [DataMember(Order = 2)] public CountryCodeType? CountryCodeType { get; set; }
        [DataMember(Order = 3)] public string CountryCode { get; set; }
        [DataMember(Order = 4)] public long? BrandId { get; set; }
        [DataMember(Order = 5)] public string Offer { get; set; }
        [DataMember(Order = 6)] public DateTime? FromDate { get; set; }
        [DataMember(Order = 7)] public DateTime? ToDate { get; set; }
        [DataMember(Order = 8)] public ReportType? ReportType { get; set; }
        [DataMember(Order = 9)] public long? Cursor { get; set; }
        [DataMember(Order = 10)] public int Take { get; set; }
        [DataMember(Order = 11)] public bool Asc { get; set; }
        [DataMember(Order = 12)] public string TenantId { get; set; }
    }
}