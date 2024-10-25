using Coravel.Invocable;
using PuanConnect.Interfaces;

namespace PuanConnect.CRONJobs;

public class CloseEventInvocable : IInvocable
{
    private readonly ILogger<CloseEventInvocable> _logger;
    private readonly IEventsRepository _eventsRepository;

    public CloseEventInvocable(ILogger<CloseEventInvocable> logger, IEventsRepository eventsRepository)
    {
        _logger = logger;
        _eventsRepository = eventsRepository;
    }

    public async Task Invoke()
    {
        await _eventsRepository.CloseEventWorker();
        _logger.LogInformation("Close event");
    }
}