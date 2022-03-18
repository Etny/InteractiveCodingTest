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
        public IList<Test> Tests { get; set; }

        public IEnumerable<(string, TestResult)> ValidateTests(TestContext context)
            => Tests.Select(t => (t.Name, t.Result(context)));

        public TestFile CreateFile() {
            Console.WriteLine(Name + " - " + TypeName + " - " + FileName);
            var path = "./" + FileName + ".cs";
            var file = File.Create(path);
            var template = Assembly.GetExecutingAssembly().GetManifestResourceStream($"DynamicCheck.Templates.{FileName}.txt");

            var reader = new StreamReader(template);
            var writer = new StreamWriter(file);
            writer.Write(reader.ReadToEnd());

            writer.Dispose(); writer.Dispose(); template.Dispose(); file.Dispose();

            return new TestFile(path);
        }
    }
}