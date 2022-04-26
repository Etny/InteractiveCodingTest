using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DynamicCheck.IO;
using System;

namespace DynamicCheck.Testing {
    internal class Stage {
        public string Name { get; set; } = String.Empty;
        public string FileName { get; set; } = String.Empty;
        public string TypeName { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public IList<Test> Tests { get; set; }


        
    }
}