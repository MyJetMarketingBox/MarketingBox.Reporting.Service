using System;
using System.Runtime.Serialization;
using Destructurama.Attributed;

namespace MarketingBox.Reporting.Service.Domain.Models.Registrations
{
    [DataContract]
    public class RegistrationGeneralInfo
    {
        [DataMember(Order = 1)]
        [LogMasked(PreserveLength = true, ShowFirst = 2, ShowLast = 2)]
        public string FirstName { get; set; }

        [DataMember(Order = 2)]
        [LogMasked(PreserveLength = true, ShowFirst = 2, ShowLast = 2)]
        public string LastName { get; set; }

        [DataMember(Order = 3)]
        [LogMasked(PreserveLength = true, ShowFirst = 2, ShowLast = 2)]
        public string Email { get; set; }

        [DataMember(Order = 4)]
        [LogMasked(PreserveLength = true, ShowFirst = 2, ShowLast = 2)]
        public string Phone { get; set; }
        
        [DataMember(Order = 5)]
        [LogMasked(PreserveLength = true, ShowFirst = 2, ShowLast = 2)]
        public string Ip { get; set; }

        [DataMember(Order = 6)]
        public DateTime CreatedAt { get; set; }

        [DataMember(Order = 7)]
        public DateTime? DepositedAt { get; set; }

        [DataMember(Order = 9)]
        public DateTime? ConversionDate { get; set; }

        [DataMember(Order = 10)]
        public string Country { get; set; }
    }
}