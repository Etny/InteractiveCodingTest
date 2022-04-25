using System.Linq;
using System.Collections.Generic;
using DynamicCheck.Testing;

namespace DynamicCheck.Validation;

internal class ProgressiveTestValidator : ITestValidator
{

    private int _progress = 0;

    public IEnumerable<(string Name, TestResult Result)> ValidateTests(IList<Test> tests, TestContext context)
    {
        var r = tests.Select((t, i) => (t.Name, new TestResult(i < _progress))).ToArray();
        _progress++;
        if(r.Select(r => r.Item2).All(r => r.Kind == TestResultKind.Success)) _progress = 0;
        return r;
    }
}