
using System.Collections.Generic;
using DynamicCheck.IO;
using DynamicCheck.Testing;

namespace DynamicCheck.Tracking;
internal class StageTracker {

    private readonly IStageProvider _stageProvider;
    private readonly IList<Stage> _stages; 
    private int _index = 0;
    
    public StageTracker(IStageProvider stageProvider, TestingLifeCycle lifeCycle, ProgressTracker progress)
    {
        _stageProvider = stageProvider;
        _stages = _stageProvider.GetStages();
        _index = progress.RecordedProgress;

        lifeCycle.OnTestProgress += () => {
            _index++;

            if(HasStage)
                lifeCycle.StartStage(CurrentStage!);
            else 
                lifeCycle.EndTest();
        };
    }

    public Stage? CurrentStage => HasStage ?_stages[_index] : null;
    public bool HasStage => _index < _stages.Count;
}