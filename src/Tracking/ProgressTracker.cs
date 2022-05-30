using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DynamicCheck.Testing;
using DynamicCheck.IO;

namespace DynamicCheck.Tracking;
internal class ProgressTracker {

    private readonly List<(DateTime?, DateTime?)> _times = new();
    public int RecordedProgress => _times.Where((s) => s.Item1 != null && s.Item2 != null).Count();

    public IEnumerable<TimeSpan> GetStageTimes()
        => _times.Select(e => e.Item2!.Value.Subtract(e.Item1!.Value));
    
    public TimeSpan LastStageTime()
        => GetStageTimes().Last();

    public TimeSpan GetTotalTime()
        => GetStageTimes().Aggregate((f, s) => f.Add(s));


    public void ReadProgress() {
        var path = "./test_progress.txt";
        if(!File.Exists(path)) return;
        
        var lines = File.ReadAllLines(path);
        foreach(var line in lines) {
            var parts = line.Split('|');
            DateTime? dt1 = parts[0] == "-" ? null : DateTime.Parse(parts[0]);
            DateTime? dt2 = parts[1] == "-" ? null : DateTime.Parse(parts[1]);
            _times.Add((dt1, dt2));
        }
    }

    public void StartStageTimer() {
        if(_times.Count > 0 && _times[^1].Item2 == null) return;
        _times.Add((DateTime.Now, null));
        WriteProgress();
    }

    public void StopStageTimer() {
        _times[^1] = (_times[^1].Item1, DateTime.Now);
        WriteProgress();
    }

    private void WriteProgress() {
        var builder = new StringBuilder();
        foreach(var (start, end) in _times) {
            var dt_str1 = start?.ToString() ?? "-";
            var dt_str2 = end?.ToString() ?? "-";
            builder.AppendLine($"{dt_str1}|{dt_str2}");
        }
        File.WriteAllText("./test_progress.txt", builder.ToString());
    }

}