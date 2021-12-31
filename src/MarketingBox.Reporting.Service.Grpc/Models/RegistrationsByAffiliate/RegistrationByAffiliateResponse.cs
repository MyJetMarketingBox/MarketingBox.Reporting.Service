using System.Runtime.Serialization;
using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Grpc.Models.Common;

namespace MarketingBox.Reporting.Service.Grpc.Models.RegistrationsByAffiliate
{
    [DataContract]
    public class RegistrationByAffiliateResponse
    {
        [DataMember(Order = 1)] public RegistrationDetails Registration { get; set; }
        [DataMember(Order = 100)] public Error Error { get; set; }
    }
}