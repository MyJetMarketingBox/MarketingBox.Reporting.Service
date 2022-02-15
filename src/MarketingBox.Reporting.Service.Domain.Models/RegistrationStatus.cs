namespace MarketingBox.Reporting.Service.Domain.Models
{
    public enum RegistrationStatus
    {
        Created = 0,
        Registered = 1, // == Declined
        Deposited = 2,
        Approved = 3,
        Declined = 4
    }
}