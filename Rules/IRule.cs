
using DynamicCheck.Testing;
using Newtonsoft.Json;

namespace DynamicCheck.Rules {
    
    
    internal interface IRule {
        bool Validate(TestContext context);
    }
}