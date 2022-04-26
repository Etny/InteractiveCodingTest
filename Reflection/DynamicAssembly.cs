
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace DynamicCheck.Reflection;

internal class DynamicAssembly {
    private readonly static Lazy<List<MetadataReference>> StandardRefs = new(() => {
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

    private readonly string _filePath;
    private Assembly _assembly = null;
    private SyntaxTree _tree = null;

    public DynamicAssembly(string filePath)
    {
        _filePath = filePath;
    }

    public Assembly Assembly => _assembly ?? throw new NullReferenceException("Compiled Assembly not found");
    public SyntaxTree Tree => _tree ?? throw new NullReferenceException("Compiled syntax tree not found");
    public string FilePath => _filePath;

    public void Recompile() {
            if(_assembly != null) {
                AssemblyLoadContext.GetLoadContext(_assembly)?.Unload();
                _assembly = null;
            }
           
            var source = SourceText.From(File.ReadAllText(_filePath));
            var parse_parameters = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest);
            var compile_parameters = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            _tree = SyntaxFactory.ParseSyntaxTree(source, parse_parameters);

            var compilation  = CSharpCompilation.Create("Test", new[] { _tree }, StandardRefs.Value, compile_parameters);
             
            using var stream = new MemoryStream();
            var result = compilation.Emit(stream);

            if(!result.Success) {
                var failures = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);
                var error_message = string.Join("\n", failures.Select(d => d.GetMessage()));
                throw new Exception(error_message);
            }
            
            stream.Seek(0, SeekOrigin.Begin);

            _assembly = new TestAssemblyLoadContext().LoadFromStream(stream);
    }

    private class TestAssemblyLoadContext : AssemblyLoadContext {
            public TestAssemblyLoadContext() : base(true) {}
    }
} 