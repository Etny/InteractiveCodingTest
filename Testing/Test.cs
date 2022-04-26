using System.Collections.Generic;
using DynamicCheck.Validation;

namespace DynamicCheck.Testing {
    internal class Test {
        public string Name { get; set; } = String.Empty;
        public string Method { get; set; } = String.Empty;
        public string InstanceMethod { get; set; } = String.Empty;
        public string TypeOverride { get; set; } = String.Empty;
        public IList<RuleDec> Rules { get; set; }

        public TestResult Result(TestContext context) {

            try {
                TestContext localContext = 
                    TypeOverride != String.Empty 
                        ?   context with { Type = context.Assembly.FindType(TypeOverride) }
                        :   context;
                
                localContext.InstanceMethod = 
                    InstanceMethod == String.Empty 
                        ?   null 
                        :   localContext.Type.FindMethod(InstanceMethod);

                localContext.Method = localContext.Type.FindMethod(Method);

                var result = true;

                foreach(var dec in Rules ?? Array.Empty<RuleDec>()) {
                    localContext.InstanceProperties = dec.WithProps;
                    result = result && dec.Validate(localContext);
                }
                return new TestResult(result);
            } catch(Exception e) {
                return new TestResult(e);
            }
        }
    }
}