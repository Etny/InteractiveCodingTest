
using DynamicCheck.Testing;

namespace DynamicCheck;

internal class TestingLifeCycle {
    public event Action OnRun;
    public event Action<Stage> OnStartStage;
    public event Action<Stage> OnStageEnd;
    public event Action OnTestEnd;

    public void Run() => OnRun?.Invoke();
    public void StartStage(Stage stage) => OnStartStage?.Invoke(stage);
    public void EndStage(Stage stage) => OnStageEnd?.Invoke(stage);
    public void EndTest() => OnTestEnd?.Invoke();
    

}