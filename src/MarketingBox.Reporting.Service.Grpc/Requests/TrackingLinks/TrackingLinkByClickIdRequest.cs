using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MarketingBox.Sdk.Common.Attributes;
using MarketingBox.Sdk.Common.Models;

namespace MarketingBox.Reporting.Service.Grpc.Requests.TrackingLinks;


[DataContract]
public class TrackingLinkByClickIdRequest : ValidatableEntity
{
    [DataMember(Order = 1), Required, AdvancedCompare(ComparisonType.GreaterThan, 0)]
    public long? ClickId { get; set; }
}