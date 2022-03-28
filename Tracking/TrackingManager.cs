using System.Linq;
using System.Collections.Generic;
using DynamicCheck.Testing;

namespace DynamicCheck.Tracking;

internal class TrackingManager {

    private readonly Dictionary<string, TimeSpan> _completionTimes = new();
    private DateTime _currentStageStart;
    public TrackingManager(TestingLifeCycle lifeCycle) {
        lifeCycle.OnStartStage += StageStart;
        lifeCycle.OnStageEnd += StageEnd;
    }

    private void StageStart(Stage stage) {
        _currentStageStart = DateTime.Now;
    }

    private void StageEnd(Stage stage) {
        var time = DateTime.Now.Subtract(_currentStageStart);
        _completionTimes[stage.Name] = time;
    }

    public TimeSpan GetTime(Stage stage) 
        => _completionTimes[stage.Name];

    public TimeSpan TotalTime() => _completionTimes.Values.Aggregate((a, b) => a.Add(b));
    
}