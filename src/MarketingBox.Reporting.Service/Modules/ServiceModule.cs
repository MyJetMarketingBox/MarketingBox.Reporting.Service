using Autofac;
using MarketingBox.Affiliate.Service.Client;
using MarketingBox.Affiliate.Service.MyNoSql.Brands;
using MarketingBox.Reporting.Service.Engines;
using MarketingBox.Reporting.Service.Postgres;
using MarketingBox.Reporting.Service.Repositories;
using MarketingBox.Reporting.Service.Subscribers;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.ServiceBus;
using MyServiceBus.Abstractions;

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

            builder.RegisterType<AffiliateAccessUpdateMessageSubscriber>()
                .As<IStartable>()
                .SingleInstance()
                .AutoActivate();

            builder.RegisterType<AffiliateAccessRemovedMessageSubscriber>()
                .As<IStartable>()
                .SingleInstance()
                .AutoActivate();

            builder.RegisterType<ReportingEngine>()
                .As<IReportingEngine>()
                .SingleInstance();
            
            builder.RegisterType<RegistrationDetailsRepository>()
                .As<IRegistrationDetailsRepository>()
                .SingleInstance();
        }
    }
}
