using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Grpc.Models.RegistrationsByAffiliate
{
    [DataContract]
    public enum RegistrationsReportType
    {
        [DataMember(Order = 1)] Registrations,
        [DataMember(Order = 2)] QFTDepositors,
        [DataMember(Order = 3)] All
    }
}