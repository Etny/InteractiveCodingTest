
using System.Collections.Generic;
using DynamicCheck.Testing;

namespace DynamicCheck.Validation;

internal interface ITestValidator {
    IEnumerable<(string Name, TestResult Result)> ValidateTests(IList<Test> tests, TestContext context);
}