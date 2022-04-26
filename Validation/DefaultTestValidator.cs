using System.Linq;
using System.Collections.Generic;
using DynamicCheck.Testing;

namespace DynamicCheck.Validation;

internal class DefaultTestValidator : ITestValidator
{
    public IEnumerable<ValidationResult> ValidateTests(IList<Test> tests, TestContext context)
        => tests.Select(t => new ValidationResult(t.Name, t.Result(context)));
}