using System.IO;
using System.Reflection;

namespace DynamicCheck.IO;

internal class FakeProjectFile {


    public readonly string FilePath;

    public FakeProjectFile() {
        FilePath = "./Test.csproj";
    }

    public void Create() {
        if(File.Exists(FilePath)) return;
            
        using var file = File.Create(FilePath);
        using var template = Assembly.GetExecutingAssembly()
                                .GetManifestResourceStream($"Template-Csproj")
                                ?? throw new FileNotFoundException($"Failed to find Manifest Resource 'DynamicCheck.Csproj.txt'");

        using var reader = new StreamReader(template);
        using var writer = new StreamWriter(file);
        writer.Write(reader.ReadToEnd());
    }  

}