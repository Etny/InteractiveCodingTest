
using DynamicCheck.IO;
using DynamicCheck.Testing;
using DynamicCheck.Tracking;
using Microsoft.Extensions.Logging;

namespace DynamicCheck;

internal class TestingLifeCycle {

    private readonly ILogger<TestingLifeCycle> _logger;
    private readonly MessageWriter _messageWriter;
    private readonly StageTracker _stageTracker;
    private readonly IStageProvider _stageProvider;
    private readonly ProgressTracker _progressTracker;
    private readonly IResultWriter _resultWriter;
    private readonly TestRunner _testRunner;

    public TestingLifeCycle(ILogger<TestingLifeCycle> logger,
                            StageTracker stageTracker,
                            ProgressTracker progressTracker,
                            IResultWriter resultWriter,
                            TestRunner testRunner,
                            MessageWriter messageWriter,
                            IStageProvider stageProvider)
    {
        _logger = logger;
        _stageTracker = stageTracker;
        _progressTracker = progressTracker;
        _resultWriter = resultWriter;
        _testRunner = testRunner;
        _messageWriter = messageWriter;
        _stageProvider = stageProvider;
    }

    public void Run() {
        _progressTracker.ReadProgress();
        _messageWriter.ShowStartUp(_stageProvider.GetStages().Count);

        new FakeProjectFile().Create();

        while(_stageTracker.HasStage) {
            StartStage(_stageTracker.CurrentStage!);
            _stageTracker.Progress();
        }

        EndTest();
    }
    public void StartStage(Stage stage) {
        _logger.LogInformation("Starting stage: {name}", stage.Name);

        _messageWriter.ShowStageStart(stage);
        _progressTracker.StartStageTimer();
        _testRunner.RunStage(stage);

        _logger.LogInformation("Ending stage {name}", stage.Name);
        
        _progressTracker.StopStageTimer();
        _messageWriter.ShowStageComplete(stage, _progressTracker.LastStageTime());
    }

    public void EndTest() { 
        _logger.LogInformation("Ending Test");
        
        _resultWriter.WriteResult(_progressTracker);
        _messageWriter.ShowCompletion();
    }
    

}