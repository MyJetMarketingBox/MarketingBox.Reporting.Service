namespace MarketingBox.Reporting.Service.Domain.Models.TrackingLinks;

public class TrackingLink
{
    public long Id { get; set; }
    public long ClickId { get; set; }
    public long BrandId { get; set; }
    public long AffiliateId { get; set; }
    public string Link { get; set; }
    public LinkParameterValues LinkParameterValues { get; set; }
    public LinkParameterNames LinkParameterNames { get; set; }
    public string UniqueId { get; set; }
        
    public long? RegistrationId { get; set; }
}