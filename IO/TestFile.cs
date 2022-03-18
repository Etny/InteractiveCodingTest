
using System;
using System.IO;

namespace DynamicCheck.IO {
    internal class TestFile {
        public readonly string FilePath;
        public DateTime LastWrite { get; private set; } = DateTime.MinValue;

        public TestFile(string filePath)
        {
            FilePath = filePath;
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