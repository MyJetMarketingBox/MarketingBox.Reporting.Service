using Autofac;
using MarketingBox.Affiliate.Service.Client;
using MarketingBox.Affiliate.Service.MyNoSql.Brands;
using MarketingBox.Reporting.Service.Subscribers;
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
            
            const string queueName = "marketingbox-reporting-service";
            builder.RegisterMyServiceBusSubscriberSingle<MarketingBox.Registration.Service.Messages.Registrations.RegistrationUpdateMessage>(
                serviceBusClient,
                Registration.Service.Messages.Topics.RegistrationUpdateTopic,
                queueName,
                TopicQueueType.Permanent);
            builder.RegisterMyServiceBusSubscriberSingle<MarketingBox.Affiliate.Service.Messages.AffiliateAccesses.AffiliateAccessUpdated>(
                serviceBusClient,
                Affiliate.Service.Messages.Topics.AffiliateAccessUpdatedTopic,
                queueName,
                TopicQueueType.Permanent);
            builder.RegisterMyServiceBusSubscriberSingle<MarketingBox.Affiliate.Service.Messages.AffiliateAccesses.AffiliateAccessRemoved>(
                serviceBusClient,
                Affiliate.Service.Messages.Topics.AffiliateAccessRemovedTopic,
                queueName,
                TopicQueueType.Permanent);
            
            builder.RegisterType<BrandNoSqlSubscriber>()
                .As<IStartable>()
                .SingleInstance()
                .AutoActivate();
        }
    }
}