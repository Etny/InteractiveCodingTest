
using DynamicCheck.IO;
using DynamicCheck.Testing;
using DynamicCheck.Tracking;
using Microsoft.Extensions.Logging;

namespace DynamicCheck;

internal class TestingLifeCycle {
    public event Action? OnRun;
    public event Action<Stage>? OnStageStart;
    public event Action<Stage>? OnStageEnd;
    public event Action? OnTestProgress;
    public event Action? OnTestEnd;
    private readonly ILogger<TestingLifeCycle> _logger;

    public TestingLifeCycle(ILogger<TestingLifeCycle> logger)
    {
        _logger = logger;
    }

    public void Run() => OnRun?.Invoke();
    public void StartStage(Stage stage) {
        _logger.LogInformation("Starting stage: {name}", stage.Name);
        OnStageStart?.Invoke(stage);
    }

    public void EndStage(Stage stage) {
        _logger.LogInformation("Ending stage {name}", stage.Name);
        OnStageEnd?.Invoke(stage);
        OnTestProgress?.Invoke();
    }

    public void EndTest() { 
        _logger.LogInformation("Ending Test");
        OnTestEnd?.Invoke();    
    }
    

}