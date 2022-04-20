
using System;
using System.IO;
using System.Reflection;
using DynamicCheck.Testing;

namespace DynamicCheck.IO {
    internal class TestFile {
        public readonly string FilePath;
        public DateTime LastWrite { get; private set; } = DateTime.MinValue;

        public TestFile(string filePath)
        {
            FilePath = filePath;
        }

        public TestFile(Stage stage){
            FilePath = "./" + stage.FileName + ".cs";
            if(File.Exists(FilePath)) return;
            
            using var file = File.Create(FilePath);
            using var template = Assembly.GetExecutingAssembly()
                                    .GetManifestResourceStream($"Template-{stage.FileName}")
                                    ?? throw new FileNotFoundException($"Failed to find Manifest Resource 'DynamicCheck.{stage.FileName}.txt'");

            using var reader = new StreamReader(template);
            using var writer = new StreamWriter(file);
            writer.Write(reader.ReadToEnd());
        }

        public bool Poll() {
            var newWrite = File.GetLastWriteTime(FilePath);

            if(newWrite > LastWrite) {
                LastWrite = newWrite;
                return true;
            }

            return false;
        }

        public string Contents() 
            => File.ReadAllText(FilePath);
    }
}