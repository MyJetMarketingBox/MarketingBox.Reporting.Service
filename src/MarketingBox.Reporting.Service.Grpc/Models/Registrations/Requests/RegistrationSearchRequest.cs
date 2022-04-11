﻿using System.Runtime.Serialization;
using MarketingBox.Reporting.Service.Domain.Models;

namespace MarketingBox.Reporting.Service.Grpc.Models.Registrations.Requests
{
    [DataContract]
    public class RegistrationSearchRequest
    {
        [DataMember(Order = 1)]
        public long? AffiliateId { get; set; }

        [DataMember(Order = 2)]
        public long? Cursor { get; set; }

        [DataMember(Order = 3)]
        public int Take { get; set; }

        [DataMember(Order = 4)]
        public bool Asc { get; set; }

        [DataMember(Order = 5)]
        public string TenantId { get; set; }
        
        [DataMember(Order = 6)] 
        public RegistrationsReportType? Type { get; set; }
    }
}