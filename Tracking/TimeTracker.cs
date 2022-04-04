using System.Linq;
using System.Collections.Generic;
using DynamicCheck.Testing;
using DynamicCheck.IO;

namespace DynamicCheck.Tracking;

internal class TimeTracker {

    private readonly Dictionary<string, TimeSpan> _completionTimes = new();
    private DateTime _currentStageStart;
    public TimeTracker(TestingLifeCycle lifeCycle) {
        lifeCycle.OnStageStart += StageStart;
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

    public IEnumerable<(string Name, TimeSpan time)> GetStageTimes()
        => _completionTimes.Select(k => (k.Key, k.Value));

    public TimeSpan TotalTime() => _completionTimes.Values.Aggregate((a, b) => a.Add(b));
    
}