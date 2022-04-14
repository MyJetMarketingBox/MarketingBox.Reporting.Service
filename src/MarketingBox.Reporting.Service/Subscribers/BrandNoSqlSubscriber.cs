using System.Collections.Generic;
using System.Linq;
using Autofac;
using AutoMapper;
using MarketingBox.Affiliate.Service.MyNoSql.Brands;
using MarketingBox.Reporting.Service.Domain.Models.Reports;
using MarketingBox.Reporting.Service.Repositories;
using MarketingBox.Reporting.Service.Repositories.Interfaces;
using MyNoSqlServer.Abstractions;

namespace MarketingBox.Reporting.Service.Subscribers;

public class BrandNoSqlSubscriber : IStartable
{
    private readonly IMyNoSqlServerDataReader<BrandNoSql> _myNoSqlServerDataReader;
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;

    public BrandNoSqlSubscriber(
        IMyNoSqlServerDataReader<BrandNoSql> myNoSqlServerDataReader,
        IBrandRepository brandRepository,
        IMapper mapper)
    {
        _myNoSqlServerDataReader = myNoSqlServerDataReader;
        _brandRepository = brandRepository;
        _mapper = mapper;
    }
    public void Start()
    {
        _myNoSqlServerDataReader.SubscribeToUpdateEvents(HandleUpdate, HandleDelete);
        HandleUpdate(_myNoSqlServerDataReader.Get());
    }

    private void HandleDelete(IReadOnlyList<BrandNoSql> obj)
    {
        _brandRepository
            .DeleteAsync(obj.Select(x => _mapper.Map<BrandEntity>(x)))
            .GetAwaiter()
            .GetResult();
    }

    private void HandleUpdate(IReadOnlyList<BrandNoSql> obj)
    {
        _brandRepository
            .CreateOrUpdateAsync(obj.Select(x => _mapper.Map<BrandEntity>(x)))
            .GetAwaiter()
            .GetResult();
    }
}