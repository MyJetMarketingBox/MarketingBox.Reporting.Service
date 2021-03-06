using System;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Client;
using MarketingBox.Reporting.Service.Grpc.Requests.Registrations;
using MarketingBox.Reporting.Service.Grpc.Requests.Reports;
using ProtoBuf.Grpc.Client;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;

            Console.Write("Press enter to start");
            Console.ReadLine();


            var factory = new ReportingServiceClientFactory("http://localhost:12350");
            var reportService = factory.GetReportService();
            var leadService = factory.GetRegistrationService();

            var searchLead = await leadService.SearchAsync(new RegistrationSearchRequest()
            {
                TenantId = "default-tenant-id",
                Asc = true,
                Cursor = null,
                Take = 50
            });

            var search = await reportService.SearchAsync(new ReportSearchRequest()
            {
                //TenantId = "default-tenant-id",
                ToDate = DateTime.UtcNow,
                Asc = true,
                Cursor = null,
                FromDate = DateTime.Parse("2021-10-07 14:03:10"),
                Take = 1000,
                //MasterAffiliateId = 9
            });

            //var resp = await  client.SayHelloAsync(new HelloRequest(){Name = "Alex"});
            //Console.WriteLine(resp?.Message);
            //
            //Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
