
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
            context.CreateInvoker()
                    .WithStdOutReader(out var reader)
                    .WithStdIn(In)
                    .Invoke();
                    
            return reader().SequenceEqual(Lines);
        }
    }
}