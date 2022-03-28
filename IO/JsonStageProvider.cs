
using System.Collections.Generic;
using System.IO;
using DynamicCheck.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DynamicCheck.IO;

internal class JsonStageProvider : IStageProvider
{

    private readonly string _path;

    private readonly Lazy<List<Stage>> _stages;
    public JsonStageProvider(string path) {
        _path = path;

        _stages = new(() => {
            var json = File.ReadAllText(_path);
            var i = JsonConvert.DeserializeObject<List<Stage>>(json, new JsonSerializerSettings() {
                ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
            i.Reverse();
            return i;
        });
    }

    public IList<Stage> GetStages()
        => _stages.Value;
}