using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace MarketingBox.Reporting.Service.Settings
{
    public class SettingsModel
    {
        [YamlProperty("MarketingBoxReportingService.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("MarketingBoxReportingService.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("MarketingBoxReportingService.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }

        [YamlProperty("MarketingBoxReportingService.MyNoSqlReaderHostPort")]
        public string MyNoSqlReaderHostPort { get; set; }

        [YamlProperty("MarketingBoxReportingService.MarketingBoxServiceBusHostPort")]
        public string MarketingBoxServiceBusHostPort { get; set; }

        [YamlProperty("MarketingBoxReportingService.PostgresConnectionString")]
        public string PostgresConnectionString { get; set; }

        [YamlProperty("MarketingBoxReportingService.AffiliateServiceUrl")]
        public string AffiliateServiceUrl { get; set; }

        [YamlProperty("MarketingBoxReportingService.JaegerUrl")]
        public string JaegerUrl { get; set; }
    }
}
