using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models.Registrations;
using MarketingBox.Reporting.Service.Grpc.Requests.Registrations;
using MarketingBox.Reporting.Service.Services.Interfaces;
using MarketingBox.Sdk.Common.Enums;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.Grpc;

namespace MarketingBox.Reporting.Service.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ILogger<RegistrationService> _logger;
        private readonly DatabaseContextFactory _databaseContextFactory;
        readonly IBrandBoxReportService _brandBoxReportService;

        public RegistrationService(ILogger<RegistrationService> logger,
            DatabaseContextFactory databaseContextFactory,
            IBrandBoxReportService brandBoxReportService)
        {
            _logger = logger;
            _databaseContextFactory = databaseContextFactory;
            _brandBoxReportService = brandBoxReportService;
        }

        public async Task<Response<IReadOnlyCollection<RegistrationDetails>>> SearchAsync(
            RegistrationSearchRequest request)
        {
            try
            {
                request.ValidateEntity();

                _logger.LogInformation(
                    "CustomerReportService.GetCustomersReport receive request : {@Request}", request);

                await using var ctx = _databaseContextFactory.Create();
                IQueryable<RegistrationDetails> query = ctx.RegistrationDetails;

                if (!string.IsNullOrWhiteSpace(request.TenantId))
                    query = query.Where(e => e.TenantId == request.TenantId);
                if (!string.IsNullOrWhiteSpace(request.FirstName))
                    query = query.Where(e => e.FirstName
                        .ToLower()
                        .Contains(request.FirstName.ToLowerInvariant()));
                if (!string.IsNullOrWhiteSpace(request.LastName))
                    query = query.Where(e => e.LastName
                        .ToLower()
                        .Contains(request.LastName.ToLowerInvariant()));
                if (!string.IsNullOrWhiteSpace(request.Email))
                    query = query.Where(e => e.Email
                        .ToLower()
                        .Contains(request.Email.ToLowerInvariant()));

                if (!string.IsNullOrWhiteSpace(request.Phone))
                    query = query.Where(e => e.Phone.Contains(request.Phone));
                if (request.AffiliateIds.Any())
                    query = query.Where(e => request.AffiliateIds.Contains(e.AffiliateId));
                if (request.Statuses.Any())
                    query = query.Where(e => request.Statuses.Contains(e.Status));
                if (request.CrmStatuses.Any())
                    query = query.Where(e => request.CrmStatuses.Contains(e.CrmStatus));
                if (request.CountryIds.Any())
                    query = query.Where(e => request.CountryIds.Contains(e.CountryId));
                if (request.RegistrationIds.Any())
                    query = query.Where(e => request.RegistrationIds.Contains(e.RegistrationId));
                if (request.IntegrationIds.Any())
                    query = query.Where(e => request.IntegrationIds.Contains(e.IntegrationId));
                if (request.BrandIds.Any())
                    query = query.Where(e => request.BrandIds.Contains(e.BrandId));
                if (request.CampaignIds.Any())
                    query = query.Where(e => request.CampaignIds.Contains(e.CampaignId));
                if (request.BrandBoxIds.Any())
                {
                    var brandIds = await _brandBoxReportService.GetBrandIdsFromBrandBoxes(request.BrandBoxIds);
                    query = query.Where(x => brandIds.Contains(x.BrandId));
                }

                if (request.OfferIds.Any())
                    query = query.Where(e => request.OfferIds.Contains(e.OfferId));

                DateTime? dateFrom = null;
                DateTime? dateTo = null;
                if (request.DateFrom.HasValue)
                {
                    dateFrom = request.DateFrom.Value.Date;
                }

                if (request.DateTo.HasValue)
                {
                    dateTo = request.DateTo.Value.Date.Add(new TimeSpan(23, 59, 59));
                }

                switch (request.DateType)
                {
                    case DateTimeType.DepositDate:
                    {
                        if (dateFrom.HasValue)
                            query = query.Where(e => e.DepositDate >= dateFrom);
                        if (dateTo.HasValue)
                            query = query.Where(e => e.DepositDate <= dateTo);
                        break;
                    }
                    case DateTimeType.ConversionDate:
                    {
                        if (dateFrom.HasValue)
                            query = query.Where(e => e.ConversionDate >= dateFrom);
                        if (dateTo.HasValue)
                            query = query.Where(e => e.ConversionDate <= dateTo);
                        break;
                    }
                    case DateTimeType.RegistrationDate:
                    {
                        if (dateFrom.HasValue)
                            query = query.Where(e => e.CreatedAt >= dateFrom);
                        if (dateTo.HasValue)
                            query = query.Where(e => e.CreatedAt <= dateTo);
                        break;
                    }
                }

                switch (request.Type)
                {
                    case RegistrationsReportType.Registrations:
                        query = query.Where(e => e.Status == RegistrationStatus.Registered);
                        break;
                    case RegistrationsReportType.Ftd:
                        query = query.Where(e => e.Status == RegistrationStatus.Approved ||
                                                 e.Status == RegistrationStatus.Deposited);
                        break;
                    case RegistrationsReportType.All:
                        break;
                }

                var total = query.Count();
                if (request.Asc.Value)
                {
                    if (request.Cursor.HasValue)
                    {
                        query = query.Where(x => x.RegistrationId > request.Cursor);
                    }

                    query = query.OrderBy(x => x.RegistrationId);
                }
                else
                {
                    if (request.Cursor.HasValue)
                    {
                        query = query.Where(x => x.RegistrationId < request.Cursor);
                    }

                    query = query.OrderByDescending(x => x.RegistrationId);
                }

                if (request.Take.HasValue)
                {
                    query = query.Take(request.Take.Value);
                }

                var result = query.ToList();

                return new Response<IReadOnlyCollection<RegistrationDetails>>()
                {
                    Status = ResponseStatus.Ok,
                    Data = result,
                    Total = total
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error happened for request {@context}", request);
                return ex.FailedResponse<IReadOnlyCollection<RegistrationDetails>>();
            }
        }
    }
}