using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Grpc.Models.Deposits.Requests
{
    [DataContract]
    public class DepositSearchRequest
    {
        [DataMember(Order = 1)]
        public long? AffiliateId { get; set; }

        [DataMember(Order = 2)]
        public long? RegistrationId { get; set; }

        [DataMember(Order = 10)]
        public long? Cursor { get; set; }

        [DataMember(Order = 11)]
        public int Take { get; set; }

        [DataMember(Order = 12)]
        public bool Asc { get; set; }

        [DataMember(Order = 13)]
        public string TenantId { get; set; }

        [DataMember(Order = 14)]
        public long? MasterAffiliateId { get; set; }
    }
}