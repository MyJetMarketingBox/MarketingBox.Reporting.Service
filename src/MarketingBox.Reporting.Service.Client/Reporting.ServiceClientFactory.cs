using JetBrains.Annotations;
using MarketingBox.Reporting.Service.Grpc;
using MyJetWallet.Sdk.Grpc;

namespace MarketingBox.Reporting.Service.Client
{
    [UsedImplicitly]
    public class ReportingServiceClientFactory: MyGrpcClientFactory
    {
        public ReportingServiceClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public IReportService GetReportService() => CreateGrpcService<IReportService>();

        public IRegistrationService GetRegistrationService() => CreateGrpcService<IRegistrationService>();
        
        public IAffiliateService GetCustomerReportService() => CreateGrpcService<IAffiliateService>();
        public ITrackingLinkReportService GetTrackingLinkReportService() => CreateGrpcService<ITrackingLinkReportService>();
    }
}
