using System.Linq;
using System.Collections.Generic;
using DynamicCheck.Testing;

namespace DynamicCheck.Rules {

    [RuleTag("or")]
    internal class OrRule : IRule
    {
        public IRule Rule1 { get; set; } = null;
        public IList<IRule> Rules1 { get; set; }
        public IRule Rule2 { get; set; } = null;
        public IList<IRule> Rules2 { get; set; }

        public bool Validate(TestContext context)
            => (Rule1?.Validate(context) ?? Rules1.All(r => r.Validate(context)))
            || (Rule2?.Validate(context) ?? Rules2.All(r => r.Validate(context)));
        
    }
}