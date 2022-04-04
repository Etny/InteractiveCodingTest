
using System.IO;
using DynamicCheck.Tracking;

namespace DynamicCheck.IO;

internal class FileResultWriter : IResultWriter {


    public void WriteResult(ProgressTracker tracking) {
        var file = File.Create("./Results.txt");
        using var writer = new StreamWriter(file);
        writer.WriteLine($"Total Time: {tracking.GetTotalTime()}");

        foreach (var time in tracking.GetStageTimes())
            writer.WriteLine(time);  
    }
}