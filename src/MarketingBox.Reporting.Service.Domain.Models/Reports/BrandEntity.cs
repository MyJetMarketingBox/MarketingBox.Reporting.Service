using MarketingBox.Sdk.Common.Enums;

namespace MarketingBox.Reporting.Service.Domain.Models.Reports;

public class BrandEntity
{
    public long Id { get; set; }
    public string TenantId { get; set; }
    public string Name { get; set; }
    
    public decimal RevenueAmount { get; set; }

    public Currency RevenueCurrency { get; set; }

    public Plan RevenuePlan { get; set; }
    
    public decimal PayoutAmount { get; set; }

    public Currency PayoutCurrency { get; set; }

    public Plan PayoutPlan { get; set; }
}