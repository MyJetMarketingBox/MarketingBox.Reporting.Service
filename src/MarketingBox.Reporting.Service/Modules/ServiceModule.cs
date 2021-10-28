using Autofac;
using MarketingBox.Affiliate.Service.Client;
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

            builder.RegisterMyNoSqlReader<CampaignNoSql>(noSqlClient, CampaignNoSql.TableName);

            #region MarketingBox.Registration.Service.Messages.Leads.LeadBusUpdatedMessage

            // subscriber (ISubscriber<MarketingBox.Registration.Service.Messages.Leads.LeadBusUpdatedMessage>)
            builder.RegisterMyServiceBusSubscriberSingle<MarketingBox.Registration.Service.Messages.Leads.LeadUpdateMessage>(
                serviceBusClient,
                MarketingBox.Registration.Service.Messages.Topics.LeadUpdateTopic,
                "marketingbox-reporting-service",
                TopicQueueType.Permanent);

            #endregion

            builder.RegisterType<LeadUpdateMessageSubscriber>()
                .SingleInstance()
                .AutoActivate();
        }
    }
}
