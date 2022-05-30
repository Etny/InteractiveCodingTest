
using System.Collections.Generic;
using DynamicCheck.Testing;

namespace DynamicCheck.Validation;

internal interface ITestValidator {
    IEnumerable<ValidationResult> ValidateTests(IList<Test> tests, TestContext context);
}