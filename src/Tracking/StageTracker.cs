
using System.Collections.Generic;
using DynamicCheck.IO;
using DynamicCheck.Testing;

namespace DynamicCheck.Tracking;
internal class StageTracker {

    private readonly IStageProvider _stageProvider;
    private readonly ProgressTracker _progress;
    private int? _index = null;

    private int Index {
        get {
            if(!_index.HasValue)
                _index = _progress.RecordedProgress;
            return _index.Value;
        } 
        set {
            _index = value;
        }
    }

    public StageTracker(IStageProvider stageProvider, ProgressTracker progress)
    {
        _stageProvider = stageProvider;
        _progress = progress;
    }

    public void Progress() {
        Index++;
    }

    public Stage CurrentStage => HasStage ? _stageProvider.GetStages()[Index] : null;
    public bool HasStage => Index < _stageProvider.GetStages().Count;
}