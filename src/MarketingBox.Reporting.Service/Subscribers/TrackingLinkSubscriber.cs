using System;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using DotNetCoreDecorators;
using MarketingBox.Reporting.Service.Repositories.Interfaces;
using MarketingBox.TrackingLink.Service.Messages;
using Microsoft.Extensions.Logging;

namespace MarketingBox.Reporting.Service.Subscribers;

public class TrackingLinkSubscriber : IStartable
{
    private readonly ILogger<TrackingLinkSubscriber> _logger;
    private readonly ITrackingLinkRepository _repository;
    private readonly IMapper _mapper;
    
    public TrackingLinkSubscriber(
        ISubscriber<TrackingLinkUpsertMessage> subscriber,
        ILogger<TrackingLinkSubscriber> logger,
        ITrackingLinkRepository repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        subscriber.Subscribe(Handle);
    }

    private async ValueTask Handle(TrackingLinkUpsertMessage message)
    {
        try
        {
            _logger.LogInformation("Processing message {@Message}", message);

            var request = _mapper.Map<Domain.Models.TrackingLinks.TrackingLink>(message.TrackingLink);
            
            await _repository.CreateOrUpdateAsync(request);
            
            _logger.LogInformation("Message {@Message} was processed", message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during processing {@Message}", message);
            throw;
        }
    }

    public void Start()
    {
    }
}