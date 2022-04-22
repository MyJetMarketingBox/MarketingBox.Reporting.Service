using Autofac;
using FluentValidation;
using MarketingBox.Reporting.Service.Engines;
using MarketingBox.Reporting.Service.Engines.Interfaces;
using MarketingBox.Reporting.Service.Grpc.Requests.Reports;
using MarketingBox.Reporting.Service.Postgres;
using MarketingBox.Reporting.Service.Repositories;
using MarketingBox.Reporting.Service.Repositories.Interfaces;
using MarketingBox.Reporting.Service.Services;
using MarketingBox.Reporting.Service.Services.Interfaces;
using MarketingBox.Reporting.Service.Subscribers;
using MarketingBox.Reporting.Service.Validators;

namespace MarketingBox.Reporting.Service.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<DatabaseContextFactory>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<RegistrationUpdateMessageSubscriber>()
                .As<IStartable>()
                .SingleInstance()
                .AutoActivate();

            builder.RegisterType<TrackingLinkSubscriber>()
                .As<IStartable>()
                .SingleInstance()
                .AutoActivate();

            builder.RegisterType<ReportingEngine>()
                .As<IReportingEngine>()
                .SingleInstance();
            
            builder.RegisterType<RegistrationDetailsRepository>()
                .As<IRegistrationDetailsRepository>()
                .SingleInstance();
            
            builder.RegisterType<BrandRepository>()
                .As<IBrandRepository>()
                .SingleInstance();
            
            builder.RegisterType<ReportSearchRequestValidator>()
                .As<IValidator<ReportSearchRequest>>()
                .SingleInstance();            
            
            builder.RegisterType<TrackingLinkRepository>()
                .As<ITrackingLinkRepository>()
                .SingleInstance();
            
            builder.RegisterType<BrandBoxReportService>()
                .As<IBrandBoxReportService>()
                .SingleInstance();
        }
    }
}
