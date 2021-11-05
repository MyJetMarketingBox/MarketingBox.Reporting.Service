using System.Collections.Generic;
using System.Runtime.Serialization;
using MarketingBox.Reporting.Service.Grpc.Models.Common;

namespace MarketingBox.Reporting.Service.Grpc.Models.Leads
{
    [DataContract]
    public class RegistrationSearchResponse
    {
        [DataMember(Order = 1)]
        public IReadOnlyCollection<Registration> Registrations { get; set; }

        [DataMember(Order = 100)]
        public Error Error { get; set; }
    }
}