using MarketingBox.Sdk.Common.Enums;

namespace MarketingBox.Reporting.Service.Domain.Models.Brands;

public class BrandEntity
{
    public string TenantId { get; set; }
    public long Id { get; set; }
    public string Name { get; set; }
    public long? IntegrationId { get; set; }
    public IntegrationType IntegrationType { get; set; }
}