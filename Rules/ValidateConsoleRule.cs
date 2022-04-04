
using System;
using System.IO;
using System.Linq;
using DynamicCheck.Testing;

namespace DynamicCheck.Rules {
    
    [RuleTag("validate_console")]
    internal class ValidateConsoleRule : IRule
    {
        public string[] Lines { get; set; } = Array.Empty<string>();
        public string[] In { get; set; } = Array.Empty<string>();

        public bool Validate(TestContext context)
        {
            var intercept = new StringWriter();
            var input = new StringReader(string.Join("\n", In));
            
            context.Method.InvokeWithTimeout(context.Instance, Array.Empty<object>(), intercept, input);

            var output = intercept.ToString().Split('\n').Where(t => t.Trim().Length > 0);
            return output.SequenceEqual(Lines);
        }
    }
}