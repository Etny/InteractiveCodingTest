using System;
using System.Linq;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace DynamicCheck.Stages {
    
    using TestList = List<(string, Func<bool>)>;

    internal abstract class Stage {

        public bool Begun { get; private set; } = false;
        protected string FileName { get ; private set; }
        protected readonly TestList Tests; 
        // protected TestList Tests { get; set; }
        private DateTime lastAccess;

        protected Assembly Assembly { get; private set; }
        protected SyntaxNode Root { get; private set; }
        protected Type Type { get; private set; }
        private readonly string TypeName;

        protected Stage(string fileName, string typeName) {
            FileName = fileName;
            TypeName = typeName;
            lastAccess = DateTime.MinValue;

            Tests = new TestList();

            foreach(var m in GetType().GetMethods())
                Console.WriteLine(m.Name);

            var tests = GetType().GetMethods().Where(m => m.GetCustomAttribute(typeof(TestAtribute)) != null);
            Console.WriteLine(tests.Count());
            Console.ReadKey();
            foreach(var test in tests)
                Tests.Add(("test", new Func<bool>(() => (bool)test.Invoke(this, Array.Empty<object>()))));
        }

        protected string FilePath => $"./{FileName}.cs";

        public void Begin() {
            if(Begun) return;
            Begun = true;

            var file = File.Create(FileName + ".cs");
            var template = Assembly.GetExecutingAssembly().GetManifestResourceStream($"DynamicCheck.TestCode.{FileName}.txt");

            var reader = new StreamReader(template);
            var writer = new StreamWriter(file);
            writer.Write(reader.ReadToEnd());

            writer.Dispose(); writer.Dispose(); template.Dispose(); file.Dispose();
        }

        public bool Poll() {
            var newAccess = File.GetLastWriteTime(FilePath);
            
            if(lastAccess == DateTime.MinValue || newAccess.Subtract(lastAccess).Milliseconds > 0) {
                lastAccess = newAccess;
                var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(FilePath));
                var assembly = AssemblyUtils.CompileStage(FilePath);

                return Update(tree, assembly);
            }

            return false;
        }

        private bool Update(SyntaxTree tree, Assembly assembly) {
            Console.Clear();

            UX.WriteFormatted($"Vooruitgang in file <DarkMagenta>{FileName}</>:\n\n");            

            try {
                Type = assembly.FindType(TypeName);
            } catch(Exception e) { 
                UX.WriteFormatted($"   <Red>{e.Message}</>.\n");
            }

            Root = tree.GetRoot();
            Assembly = assembly;

            if(Assembly == null) {
                UX.WriteFormatted("   <Red>Er zijn errors in je file. Los ze op om verder te gaan</>.\n");
                return false;
            }

            return CheckCompleted();
        }

        protected bool CheckCompleted() {
            var result = true;

            
            foreach(var test in Tests) {
                string test_message = "";
                try {
                    var test_result = test.Item2();
                    result = result && test_result;
                    
                    test_message = test_result 
                        ? "<Green>\u2714</>"
                        : "<Red>\u2718</>";
                } catch(Exception e) {
                    test_message = "<DarkRed>Er is iets fout gegaan: <Red>" + e.Message + "</>";
                    result = false;
                }
                
                UX.WriteFormatted("   " + test.Item1 + ": " + test_message + "\n");
            }

            return result;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    internal class TestAtribute: Attribute {
        public readonly string Name;

        public TestAtribute(string name) {
            Name = name;
        }
    }
}