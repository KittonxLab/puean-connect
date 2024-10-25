using Coravel.Invocable;
using PuanConnect.Interfaces;

namespace PuanConnect.CRONJobs;

public class GainRPInvocable : IInvocable
{
    private readonly ILogger<GainRPInvocable> _logger;
    private readonly IUsersRepository _usersRepository;


    public GainRPInvocable(ILogger<GainRPInvocable> logger, IUsersRepository usersRepository)
    {
        _logger = logger;
        _usersRepository = usersRepository;
    }

    public async Task Invoke()
    {
        await _usersRepository.GainUsersRPWorker();
        _logger.LogInformation("Gain RP");
    }
}