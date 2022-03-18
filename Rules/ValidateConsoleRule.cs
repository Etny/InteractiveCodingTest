
using System;
using System.IO;
using System.Linq;
using DynamicCheck.Testing;

namespace DynamicCheck.Rules {
    
    [RuleTag("validate_console")]
    internal class ValidateConsoleRule : IRule
    {
        public string[] Lines { get; set; }
        public string[] In { get; set; } = Array.Empty<string>();

        public bool Validate(TestContext context)
        {
            var oldOut = Console.Out;
            var oldIn = Console.In;
            
            var intercept = new StringWriter();
            Console.SetOut(intercept);

            var input = new StringReader(string.Join("\n", In));
            Console.SetIn(input);
            
            context.Method.Invoke(null, Array.Empty<object>());

            Console.SetOut(oldOut);
            Console.SetIn(oldIn);

            var output = intercept.ToString().Split('\n').Where(t => t.Trim().Length > 0);
            return output.SequenceEqual(Lines);
        }
    }
}