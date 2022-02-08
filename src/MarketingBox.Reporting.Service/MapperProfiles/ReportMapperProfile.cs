using AutoMapper;
using MarketingBox.Reporting.Service.Domain.Models.Reports;
using MarketingBox.Reporting.Service.Grpc.Models.Reports;
using MarketingBox.Reporting.Service.Repositories;

namespace MarketingBox.Reporting.Service.MapperProfiles;

public class ReportMapperProfile : Profile
{
    public ReportMapperProfile()
    {
        CreateMap<Grpc.Models.Reports.Requests.ReportSearchRequest, ReportSearchRequest>();
        CreateMap<AggregatedReportEntity, Report>();
    }
}