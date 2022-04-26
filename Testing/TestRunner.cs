
using System.Collections.Generic;
using System.Threading;
using DynamicCheck.IO;
using DynamicCheck.Reflection;
using DynamicCheck.Validation;
using Microsoft.Extensions.Logging;

namespace DynamicCheck.Testing {
    internal class TestRunner {
 
        private readonly MessageWriter _ux;
        private readonly ITestValidator _validator;
        private readonly ILogger<TestRunner> _logger;


        public TestRunner(MessageWriter ux,  ITestValidator validator, ILogger<TestRunner> logger)
        {
            _ux = ux;
            _validator = validator;
            _logger = logger;
        }

        public void RunStage(Stage stage) {            
            _logger.LogDebug("Creating file for stage {name}", stage.Name);
            var file = new TestFile(stage);

            var assembly = new DynamicAssembly(file.FilePath);

            while(true) {
                if(file.Poll() && Update(stage, assembly)) {
                    return;
                } else 
                    Thread.Sleep(200);    
            }
        }

        private bool Update(Stage stage, DynamicAssembly dynAssembly) {
            Console.Clear();
            _ux.WriteFormatted($"Vooruitgang in File <DarkMagenta>{stage.FileName}.cs</> (save de file om verandering door te geven):\n\n");            

            var context = new TestContext();

            try{
                _logger.LogDebug("Recompiling {file}", dynAssembly.FilePath);
                dynAssembly.Recompile();
                context.Assembly = dynAssembly.Assembly;
                context.Root = dynAssembly.Tree.GetRoot();
                context.Type = context.Assembly.FindType(stage.TypeName);
            } catch(Exception e) {
                _ux.WriteFormatted($"   <DarkRed>Er is een error in je file: <Red>{e.Message}</>.\n");
                return false;
            }

            _logger.LogDebug("Validating tests...");

            var failedTestFuncs = new List<string>();

            bool result = true;
            foreach(var validationResult in _validator.ValidateTests(stage.Tests, context)) {
                result = result && validationResult.Result.Kind == TestResultKind.Success;
                _ux.WriteFormatted("   " + validationResult.TestName + ": " + validationResult.Result.Display + "\n");
                if(validationResult.Result.Kind != TestResultKind.Success)
                    failedTestFuncs.Add(validationResult.TestName);
                
            }
           

            // if(failedTestFuncs.Count > 0) 
            //     _ux.WriteFormatted($"\n<Yellow>Debug</> output:\n   {string.Join("\n   ", failedTestFuncs)}");
            

            return result;
        }

        
    }
}