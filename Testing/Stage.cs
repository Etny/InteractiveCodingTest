
using System.Collections.Generic;

namespace DynamicCheck.Testing {
    internal class Stage {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public IList<Test> Tests { get; set; }
    }
}