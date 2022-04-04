
using System.Collections.Generic;
using DynamicCheck.Rules;
using Newtonsoft.Json;

namespace DynamicCheck.Testing {
    
    [JsonConverter(typeof(RuleDecJsonConverter))]
    internal class RuleDec {
        public IRule? Rule { get; set; }
        public List<PropertyDec> WithProps { get; set; } = new List<PropertyDec>();


        public bool Validate(TestContext context) {
            context.InstanceProperties = WithProps;
            return Rule?.Validate(context) ?? true;
        }
    }
}