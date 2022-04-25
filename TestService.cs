
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicCheck.Testing;
using DynamicCheck.Tracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DynamicCheck;
internal class TestService : IHostedService
{

    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly TestingLifeCycle _lifeCycle;
    private int? _result = null;

    public TestService(IHostApplicationLifetime lifetime,
                        ILogger<TestService> logger, 
                        TestingLifeCycle lifeCycle)
    {
        _lifetime = lifetime;
        _logger = logger;
        _lifeCycle = lifeCycle;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting application...");

        // var args = Environment.GetCommandLineArgs();
        // var secured = args.Contains("-p");
        // var verify = args.Contains("-v");

        _lifetime.ApplicationStarted.Register(() => {
            Task.Run(() => {
                try{
                    _lifeCycle.Run();
                    _result = 0;
                } catch (Exception e) {
                    _logger.LogError(e, "Uncaught Error");
                    _result = 1;     
                } finally {
                    _lifetime.StopApplication();
                }
            });
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Environment.ExitCode = _result.GetValueOrDefault(-1);
        return Task.CompletedTask;
    }
}