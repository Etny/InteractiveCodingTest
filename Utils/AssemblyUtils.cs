
using System;
using System.Linq;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Collections.Generic;

namespace DynamicCheck {
    internal static class AssemblyUtils {

        private static Assembly? _assembly = null;

        private readonly static Lazy<List<MetadataReference>> _references = new(() => {
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location),
            };

            Assembly.GetEntryAssembly()!.GetReferencedAssemblies().ToList()
                .ForEach(r => references.Add(MetadataReference.CreateFromFile(Assembly.Load(r).Location)));

            return references;
        });

        public static (SyntaxTree, Assembly) CompileStage(string filePath) {

             if(_assembly != null) {
                AssemblyLoadContext.GetLoadContext(_assembly)?.Unload();
                // for (var i = 0; i < 8; i++)
                // {
                //     GC.Collect();
                //     GC.WaitForPendingFinalizers();
                // }
                _assembly = null;
            }
           
            var source = SourceText.From(File.ReadAllText(filePath));
            var parse_parameters = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest);
            var compile_parameters = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            var tree = SyntaxFactory.ParseSyntaxTree(source, parse_parameters);

            var compilation  = CSharpCompilation.Create("Test", new[] { tree }, _references.Value, compile_parameters);
             
            using var stream = new MemoryStream();
            var result = compilation.Emit(stream);

            if(!result.Success) {
                var failures = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);
                var error_message = string.Join("\n", failures.Select(d => d.GetMessage()));
                throw new Exception(error_message);
            }
            
            stream.Seek(0, SeekOrigin.Begin);

            _assembly = new TestAssemblyLoadContext().LoadFromStream(stream);

            return (tree, _assembly);
        }

        public static object InvokeWithTimeout(
            this MethodInfo method,
            object obj,
            object[] args,
            TextWriter? stdOutHook = null,
            TextReader? stdInHook = null
        ) {
            var stdOut = Console.Out;
            var stdIn = Console.In;


            stdOutHook ??= new StringWriter();
            stdInHook ??= new StringReader("");

            Console.SetOut(stdOutHook);
            Console.SetIn(stdInHook);

            try { 
                var task = Task.Run(() => method.Invoke(obj, args));
                var result = task.Wait(TimeSpan.FromMilliseconds(100));

                if(result)
                    if(task.Exception != null)
                        throw new Exception("Exception in je code: " + task.Exception.Message);
                    else
                        return task.Result!;
                else
                    throw new Exception("Timed-out. Zit er een infinite loop in je code?");
            } finally {
                Console.SetOut(stdOut);
                Console.SetIn(stdIn);
            }

        }

        public static PropertyInfo FindProperty(this Type type, string name/*, params Type[] args*/)
            => type.GetProperties().FirstOrDefault(p => p.Name == name)
                ?? throw new Exception($"Kan property {name} niet vinden in class {type.Name}. Heb je hem weggegooid?");
        
        public static MethodInfo FindMethod(this Type type, string name/*, params Type[] args*/)
            => type.GetMethods().FirstOrDefault(m => m.Name == name)
                ?? throw new Exception($"Kan method {name} niet vinden in class {type.Name}. Heb je hem weggegooid?");

        public static Type FindType(this Assembly assembly, string name)
            => assembly.GetTypes().Where(t => t.Name == name).FirstOrDefault()
                ?? throw new Exception($"Kan class {name} niet vinden. Heb je hem weggegooid?");
        

        private class TestAssemblyLoadContext : AssemblyLoadContext {
            public TestAssemblyLoadContext() : base(true) {}

        }
    }
}