using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Domain.Models.Enums
{
    [DataContract]
    public enum RegistrationsReportType
    {
        Registrations,
        Ftd,
        All
    }
}