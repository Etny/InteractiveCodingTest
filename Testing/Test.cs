using System.Linq;
using System.Collections.Generic;
using DynamicCheck.Rules;
using System;

namespace DynamicCheck.Testing {
    internal class Test {
        public string Name { get; set; }
        public string Method { get; set; }
        public IList<IRule> Rules { get; set; }

        public TestResult Result(TestContext context) {

            try {
                var method = context.Type.FindMethod(Method);
                context.Method = method;

                var result = true;

                foreach(var rule in Rules)
                    result = result && rule.Validate(context);

                return new TestResult(result);
            } catch(Exception e) {
                return new TestResult(e);
            }
        }
    }
}