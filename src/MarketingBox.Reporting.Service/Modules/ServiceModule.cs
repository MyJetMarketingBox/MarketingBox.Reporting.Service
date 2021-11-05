using Autofac;
using MarketingBox.Affiliate.Service.Client;
using MarketingBox.Affiliate.Service.MyNoSql.Brands;
using MarketingBox.Affiliate.Service.MyNoSql.Campaigns;
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
            builder.RegisterAffiliateServiceClient(Program.Settings.AffiliateServiceUrl);

            var noSqlClient = builder.CreateNoSqlClient(Program.ReloadedSettings(e => e.MyNoSqlReaderHostPort));

            var serviceBusClient = builder
                .RegisterMyServiceBusTcpClient(
                    Program.ReloadedSettings(e => e.MarketingBoxServiceBusHostPort), 
                    Program.LogFactory);

            builder.RegisterMyNoSqlReader<BrandNoSql>(noSqlClient, BrandNoSql.TableName);

            #region MarketingBox.Registration.Service.Messages.Registrations.RegistrationUpdateMessage

            // subscriber (ISubscriber<MarketingBox.Registration.Service.Messages.Registrations.RegistrationUpdateMessage>)
            builder.RegisterMyServiceBusSubscriberSingle<MarketingBox.Registration.Service.Messages.Registrations.RegistrationUpdateMessage>(
                serviceBusClient,
                MarketingBox.Registration.Service.Messages.Topics.RegistrationUpdateTopic,
                "marketingbox-reporting-service",
                TopicQueueType.Permanent);

            #endregion

            builder.RegisterType<RegistrationUpdateMessageSubscriber>()
                .SingleInstance()
                .AutoActivate();
        }
    }
}
