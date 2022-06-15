
using System.Collections.Generic;
using System.Threading;
using System.Linq;
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
            file.Create();

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

            var testResults = _validator.ValidateTests(stage.Tests, context).ToArray();

            foreach(var result in testResults)
                _ux.WriteFormatted($"   {result.TestName}: {result.Result.Display}\n");   

            var failedWithDebug = testResults.Where(
                    r => r.Result.Kind != TestResultKind.Success && 
                    r.Result.DebugLines.Count > 0
                ).ToArray();

            if(failedWithDebug.Length > 0) {
                _ux.WriteFormatted("\n<DarkYellow>Debug</> output (van Console.WriteLine):\n");

                foreach(var failed in failedWithDebug)
                {
                    _ux.WriteFormatted($"   <Yellow>{failed.TestName}</>:\n");

                    foreach(var line in failed.Result.DebugLines)
                        _ux.WriteFormatted($"      {line}\n");
                }
            }
          

            return testResults.All(r => r.Result.Kind == TestResultKind.Success);
        }

        
    }
}