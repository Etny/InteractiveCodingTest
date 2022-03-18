
using System;

namespace DynamicCheck.Testing {
    internal enum TestResultKind {
        Success,
        Fail,
        Error
    }

    internal struct TestResult {
        public readonly TestResultKind Kind;
        public readonly string Display;

        public TestResult(bool success) {
            if(success) {
                Kind = TestResultKind.Success;
                Display = "<Green>\u2714</>";
            } else {
                Kind = TestResultKind.Fail;
                Display = "<Red>\u2718</>";
            }
        }

        public TestResult(Exception e) {
            Kind = TestResultKind.Error;;
            Display = "<DarkRed>Er is iets fout gegaan: <Red>" + e.Message + "</>";
        }
    }
}