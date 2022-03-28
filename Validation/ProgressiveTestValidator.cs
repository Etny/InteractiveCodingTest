using System.Linq;
using System.Collections.Generic;
using DynamicCheck.Testing;

namespace DynamicCheck.Validation;

internal class ProgressiveTestValidator : ITestValidator
{

    private int _progress = 0;

    public ProgressiveTestValidator(TestingLifeCycle cycle) {
        cycle.OnStageStart += _ => _progress = 0;
    }


    public IEnumerable<(string Name, TestResult Result)> ValidateTests(IList<Test> tests, TestContext context)
    {
        var r = tests.Select((t, i) => (t.Name, new TestResult(i < _progress)));
        _progress++;
        return r;
    }
}