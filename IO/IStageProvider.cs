
using System.Collections.Generic;
using DynamicCheck.Testing;

namespace DynamicCheck.IO;

internal interface IStageProvider {
    IList<Stage> GetStages();

}