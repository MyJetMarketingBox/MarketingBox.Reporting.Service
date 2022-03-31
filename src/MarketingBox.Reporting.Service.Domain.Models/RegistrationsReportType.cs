using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Domain.Models
{
    [DataContract]
    public enum RegistrationsReportType
    {
        [DataMember(Order = 1)] Registrations,
        [DataMember(Order = 2)] Ftd,
        [DataMember(Order = 3)] All
    }
}