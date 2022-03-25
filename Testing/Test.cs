using System.Linq;
using System.Collections.Generic;
using DynamicCheck.Rules;
using System;

namespace DynamicCheck.Testing {
    internal class Test {
        public string Name { get; set; }
        public string Method { get; set; }
        public string InstanceMethod { get; set; } = null;
        public string TypeOverride { get; set; } = null;
        public IList<RuleDec> Rules { get; set; }

        public TestResult Result(TestContext context) {

            try {
                var localContext = 
                    TypeOverride != null ?
                        context with { Type = context.Assembly.FindType(TypeOverride) }
                    :
                        context;
                
                localContext.InstanceMethod = 
                    InstanceMethod == null ? 
                        null 
                    : 
                        localContext.Type.FindMethod(InstanceMethod);

                localContext.Method = localContext.Type.FindMethod(Method);

                var result = true;

                foreach(var dec in Rules) {
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