using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DynamicCheck.IO;
using System;

namespace DynamicCheck.Testing {
    internal class Stage {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public IList<Test> Tests { get; set; }


        
    }
}