
using System.IO;
using DynamicCheck.Tracking;

namespace DynamicCheck.IO;

internal class FileResultWriter : IResultWriter {

    private readonly TrackingManager _tracking;

    public FileResultWriter(TrackingManager tracking)
    {
        _tracking = tracking;
    }

    public void WriteResult() {
        var file = File.Create("./Results.txt");
        using var writer = new StreamWriter(file);
        writer.Write($"Time: {_tracking.TotalTime()}");
    }
}