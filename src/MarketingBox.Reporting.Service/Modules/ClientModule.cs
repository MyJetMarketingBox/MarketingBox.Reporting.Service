using Autofac;
using MarketingBox.Affiliate.Service.Client;
using MarketingBox.Affiliate.Service.MyNoSql.Brands;
using MarketingBox.Registration.Service.Messages.Registrations;
using MarketingBox.Reporting.Service.Subscribers;
using MarketingBox.TrackingLink.Service.Messages;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.ServiceBus;
using MyServiceBus.Abstractions;

namespace MarketingBox.Reporting.Service.Modules
{
    public class ClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAffiliateServiceClient(Program.Settings.AffiliateServiceUrl);
            
            var noSqlClient = builder.CreateNoSqlClient(Program.ReloadedSettings(e => e.MyNoSqlReaderHostPort));
            builder.RegisterMyNoSqlReader<BrandNoSql>(noSqlClient, BrandNoSql.TableName);
            
            var serviceBusClient = builder
                .RegisterMyServiceBusTcpClient(
                    Program.ReloadedSettings(e => e.MarketingBoxServiceBusHostPort), 
                    Program.LogFactory);
            builder.RegisterCountryClient(Program.Settings.AffiliateServiceUrl, noSqlClient);
            
            const string queueReportingName = "marketingbox-reporting-service-local";
            builder.RegisterMyServiceBusSubscriberSingle<RegistrationUpdateMessage>(
                serviceBusClient,
                RegistrationUpdateMessage.Topic,
                queueReportingName,
                TopicQueueType.Permanent);
            
            const string queueTrackingLinkName = "marketingbox-reporting-service-tracking-link";
            builder.RegisterMyServiceBusSubscriberSingle<TrackingLinkUpsertMessage>(
                serviceBusClient,
                TrackingLinkUpsertMessage.Topic,
                queueTrackingLinkName,
                TopicQueueType.PermanentWithSingleConnection);
            
            // builder.RegisterType<BrandNoSqlSubscriber>()
            //     .As<IStartable>()
            //     .SingleInstance()
            //     .AutoActivate();
        }
    }
}