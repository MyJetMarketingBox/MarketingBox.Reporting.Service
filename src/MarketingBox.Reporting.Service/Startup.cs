using Autofac;
using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Modules;
using MarketingBox.Reporting.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyJetWallet.Sdk.GrpcSchema;
using MyJetWallet.Sdk.Service;
using Prometheus;
using SimpleTrading.ServiceStatusReporterConnector;
using System.Reflection;
using MarketingBox.Reporting.Service.Postgres;
using MyJetWallet.Sdk.Postgres;
using SimpleTrading.Telemetry;

namespace MarketingBox.Reporting.Service
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.BindCodeFirstGrpc();

            services.AddHostedService<ApplicationLifetimeManager>();

            MyDbContext.LoggerFactory = Program.LogFactory;
            services.AddDatabase(DatabaseContext.Schema,
                Program.Settings.PostgresConnectionString,
                o => new DatabaseContext(o));

            services.BindTelemetry("ReportingService", "MB-", Program.Settings.JaegerUrl);

            services.AddAutoMapper(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseMetricServer();

            app.BindServicesTree(Assembly.GetExecutingAssembly());

            app.BindIsAlive();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcSchema<ReportService, IReportService>();
                endpoints.MapGrpcSchema<RegistrationService, IRegistrationService>();
                
                endpoints.MapGrpcSchema<AffiliateService, IAffiliateService>();
                endpoints.MapGrpcSchema<TrackingLinkReportService, ITrackingLinkReportService>();

                endpoints.MapGrpcSchemaRegistry();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<SettingsModule>();
            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule<ClientModule>();
        }
    }
}
