
using DynamicCheck.Tracking;

namespace DynamicCheck.IO;

internal interface IResultWriter {
    void WriteResult(ProgressTracker tracker);
}