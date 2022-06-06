using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MarketingBox.Sdk.Common.Attributes;
using MarketingBox.Sdk.Common.Enums;
using MarketingBox.Sdk.Common.Models;

namespace MarketingBox.Reporting.Service.Grpc.Requests.Registrations
{
    [DataContract]
    public class RegistrationSearchRequest : ValidatableEntity
    {
        [DataMember(Order = 1)] public List<long> AffiliateIds { get; set; } = new ();
        [DataMember(Order = 2)] public long? Cursor { get; set; }
        [DataMember(Order = 3)] public int? Take { get; set; }
        [DataMember(Order = 4)] public bool? Asc { get; set; } = false;
        [DataMember(Order = 5)] public string? TenantId { get; set; }
        [DataMember(Order = 6), Required, IsEnum] public RegistrationsReportType? Type { get; set; }
        [DataMember(Order = 7)] public List<int> CountryIds { get; set; } = new ();
        [DataMember(Order = 8)] public List<RegistrationStatus> Statuses { get; set; } = new ();
        [DataMember(Order = 9)] public List<CrmStatus> CrmStatuses { get; set; } = new ();
        [DataMember(Order = 10)] public DateTime? DateFrom { get; set; }
        [DataMember(Order = 11)] public DateTime? DateTo { get; set; }
        [DataMember(Order = 12)] public List<long> RegistrationIds { get; set; } = new ();
        [DataMember(Order = 13)] public List<long> BrandBoxIds { get; set; } = new ();
        [DataMember(Order = 14)] public List<long?> IntegrationIds { get; set; } = new ();
        [DataMember(Order = 15)] public List<long> BrandIds { get; set; } = new ();
        [DataMember(Order = 16)] public List<long> CampaignIds { get; set; } = new ();
        [DataMember(Order = 17)] public string FirstName { get; set; }
        [DataMember(Order = 18)] public string LastName { get; set; }
        [DataMember(Order = 19)] public string Email { get; set; }
        [DataMember(Order = 20)] public string Phone { get; set; }
        [DataMember(Order = 16)] public List<long?> OfferIds { get; set; } = new ();
        [DataMember(Order = 11)] public DateTimeType? DateType { get; set; } = DateTimeType.RegistrationDate;
    }

    public enum DateTimeType
    {
        RegistrationDate,
        DepositDate,
        ConversionDate
    }
}