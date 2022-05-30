using System.Reflection;
using System.Collections.Generic;
using System.IO;
using DynamicCheck.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DynamicCheck.IO;

internal class JsonStageProvider : IStageProvider
{

    // private readonly string _path;

    private readonly Lazy<List<Stage>> _stages;
    public JsonStageProvider(string name) {

        _stages = new(() => {
            using var json_stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{name}.json")
                                        ?? throw new FileNotFoundException($"Failed to find Manifest Resource 'DynamicCheck.{name}.json'");
            using var json_reader = new StreamReader(json_stream);
            var json = json_reader.ReadToEnd();

            var i = JsonConvert.DeserializeObject<List<Stage>>(json, new JsonSerializerSettings() {
                ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });

            return i ?? throw new JsonException("Failed to deserialize stage list");
        });
    }

    public IList<Stage> GetStages()
        => _stages.Value;
}