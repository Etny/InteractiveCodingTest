
using DynamicCheck.Testing;
using Newtonsoft.Json;

namespace DynamicCheck.Rules {
    
    [JsonConverter(typeof(RuleJsonConverter))]
    interface IRule {
        bool Validate(TestContext context);
    }
}