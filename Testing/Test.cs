
using System.Collections.Generic;
using DynamicCheck.Rules;

namespace DynamicCheck.Testing {
    internal class Test {
        public string Name { get; set; }
        public IList<IRule> Rules { get; set; }
    }
}