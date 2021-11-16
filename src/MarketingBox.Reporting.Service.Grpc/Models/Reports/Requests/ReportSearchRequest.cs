using System;
using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Grpc.Models.Reports.Requests
{
    [DataContract]
    public class ReportSearchRequest
    {
        [DataMember(Order = 1)]
        public DateTime FromDate { get; set; }

        [DataMember(Order = 2)]
        public DateTime ToDate { get; set; }

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