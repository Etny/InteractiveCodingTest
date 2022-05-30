
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DynamicCheck.Validation {
    internal enum TestResultKind {
        Success,
        Fail,
        Error
    }

    internal struct TestResult {
        public readonly TestResultKind Kind;
        public readonly string Display;
        public List<string> DebugLines = new();

        public TestResult(bool success) {
            bool windows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if(success) {
                Kind = TestResultKind.Success;
                Display = windows ? "<Green>Success</>" : "<Green>\u2714</>";
            } else {
                Kind = TestResultKind.Fail;
                Display = windows ? "<Red>Failed</>" : "<Red>\u2718</>";
            }
        }

        public TestResult(Exception e) {
            Kind = TestResultKind.Error;;
            Display = "<DarkRed>Er is iets fout gegaan: <Red>" + e.Message + "</>";
        }
    }
}