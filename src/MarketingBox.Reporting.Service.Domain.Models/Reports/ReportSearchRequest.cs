using System;

namespace MarketingBox.Reporting.Service.Domain.Models.Reports
{
    public record ReportSearchRequest(
        long? AffiliateId,
        string Country,
        long? BrandId,
        string Offer,
        DateTime? FromDate,
        DateTime? ToDate,
        ReportType ReportType, 
        long? Cursor, 
        int Take, 
        bool Asc);
}