using System.Linq;
using System.Collections.Generic;
using DynamicCheck.Testing;

namespace DynamicCheck.Validation;

internal class DefaultTestValidator : ITestValidator
{
    public IEnumerable<(string Name, TestResult Result)> ValidateTests(IList<Test> tests, TestContext context)
        => tests.Select(t => (t.Name, t.Result(context)));
}