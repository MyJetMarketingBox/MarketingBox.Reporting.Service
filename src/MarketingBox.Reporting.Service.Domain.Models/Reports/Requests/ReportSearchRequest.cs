using System;
using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Domain.Models.Reports.Requests
{
    [DataContract]
    public class ReportSearchRequest
    {
        [DataMember(Order = 1)] public long? AffiliateId { get; set; }
        [DataMember(Order = 2)] public string Country { get; set; }
        [DataMember(Order = 3)] public long? BrandId { get; set; }
        [DataMember(Order = 4)] public string Offer { get; set; }
        [DataMember(Order = 5)] public DateTime? FromDate { get; set; }
        [DataMember(Order = 6)] public DateTime? ToDate { get; set; }
        [DataMember(Order = 7)] public ReportType ReportType { get; set; }
        [DataMember(Order = 8)] public long? Cursor { get; set; }
        [DataMember(Order = 9)] public int Take { get; set; }
        [DataMember(Order = 10)] public bool Asc { get; set; }
    }
}