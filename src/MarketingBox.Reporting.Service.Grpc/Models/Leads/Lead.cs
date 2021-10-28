using MarketingBox.Reporting.Service.Domain.Models.Lead;
using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Grpc.Models.Leads
{
    [DataContract]
    public class Lead
    {
        [DataMember(Order = 1)]
        public string TenantId { get; set; }

        [DataMember(Order = 2)]
        public long LeadId { get; set; }

        [DataMember(Order = 3)]
        public string UniqueId { get; set; }
        
        [DataMember(Order = 4)]
        public long Sequence { get; set; }

        [DataMember(Order = 5)]
        public LeadGeneralInfo GeneralInfo { get; set; }

        [DataMember(Order = 6)]
        public LeadRouteInfo RouteInfo { get; set; }

        [DataMember(Order = 7)]
        public LeadAdditionalInfo AdditionalInfo { get; set; }

        [DataMember(Order = 8)] 
        public LeadStatus Status  { get; set; }

        [DataMember(Order = 9)]
        public LeadCrmStatus CrmStatus{ get; set; }


    }
}
