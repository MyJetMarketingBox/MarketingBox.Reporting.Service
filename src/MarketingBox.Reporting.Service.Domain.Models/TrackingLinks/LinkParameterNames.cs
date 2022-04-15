using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Domain.Models.TrackingLinks;

[DataContract]
public class LinkParameterNames
{
    [DataMember(Order = 1)] public string ClickId { get; set; }
    
    [DataMember(Order = 2)] public string Language { get; set; }

    [DataMember(Order = 3)] public string MPC_1 { get; set; }

    [DataMember(Order = 4)] public string MPC_2 { get; set; }

    [DataMember(Order = 5)] public string MPC_3 { get; set; }

    [DataMember(Order = 6)] public string MPC_4 { get; set; }
}