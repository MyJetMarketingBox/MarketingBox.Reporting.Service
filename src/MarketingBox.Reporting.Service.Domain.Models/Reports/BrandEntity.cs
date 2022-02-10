namespace MarketingBox.Reporting.Service.Domain.Models.Reports;

public class BrandEntity
{
    public long Id { get; set; }
    public string TenantId { get; set; }
    public string Name { get; set; }
    public Payout Payout { get; set; }
    public Revenue Revenue { get; set; }
}

public class Payout
{
    public decimal Amount { get; set; }

    public Currency Currency { get; set; }

    public Plan Plan { get; set; }
}

public class Revenue
{
    public decimal Amount { get; set; }

    public Currency Currency { get; set; }

    public Plan Plan { get; set; }
}

public enum Plan
{
    CPA,
    CPL,
    CPC,
}

public enum Currency
{
    USD,
    EUR,
    GBP,
    CHF,
    BTC,
}