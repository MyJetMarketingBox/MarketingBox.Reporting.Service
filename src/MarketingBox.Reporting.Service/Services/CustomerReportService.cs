using System;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Grpc.Models;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarketingBox.Reporting.Service.Services
{
    public class CustomerReportService : ICustomerReportService
    {
        private readonly ILogger<CustomerReportService> _logger;
        private readonly DatabaseContextFactory _databaseContextFactory;

        public CustomerReportService(ILogger<CustomerReportService> logger, 
            DatabaseContextFactory databaseContextFactory)
        {
            _logger = logger;
            _databaseContextFactory = databaseContextFactory;
        }

        public async Task<GetCustomersReportResponse> GetCustomersReport(GetCustomersReportRequest request)
        {
            try
            {
                _logger.LogInformation($"CustomerReportService.GetCustomersReport receive request : {JsonConvert.SerializeObject(request)}");
                
                await using var ctx = _databaseContextFactory.Create();
                var query = ctx.Customers
                    .Where(e => e.CreatedDate >= request.From && 
                                        e.CreatedDate <= request.To);
                switch (request.Type)
                {
                    case CustomersReportType.Leads:
                        query = query.Where(e => !e.IsDeposit);
                        break;
                    case CustomersReportType.Deposits:
                        query = query.Where(e => e.IsDeposit);
                        break;
                    case CustomersReportType.LeadsAndDeposits:
                        break;
                    default:
                        throw new Exception("Something wrong with CustomersReportType at switch construction.");
                }
                var customers = query.ToList();
                if (customers.Any())
                    return new GetCustomersReportResponse()
                    {
                        Success = true,
                        Customers = customers
                    };
                throw new Exception("Cannot find customers at selected range");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new GetCustomersReportResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}