using MarketingBox.Reporting.Service.Domain.Crm;
using MarketingBox.Reporting.Service.Domain.Registrations;
using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Grpc.Models.Leads
{
    [DataContract]
    public class Registration
    {
        [DataMember(Order = 1)]
        public string TenantId { get; set; }

        [DataMember(Order = 2)]
        public long RegistrationId { get; set; }

        [DataMember(Order = 3)]
        public string UniqueId { get; set; }
        
        [DataMember(Order = 4)]
        public long Sequence { get; set; }

        [DataMember(Order = 5)]
        public RegistrationGeneralInfo GeneralInfo { get; set; }

        [DataMember(Order = 6)]
        public RegistrationRouteInfo RouteInfo { get; set; }

        [DataMember(Order = 7)]
        public RegistrationAdditionalInfo AdditionalInfo { get; set; }

        [DataMember(Order = 8)] 
        public RegistrationStatus Status  { get; set; }

        [DataMember(Order = 9)]
        public CrmStatus CrmStatus{ get; set; }


    }
}
