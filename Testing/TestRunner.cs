
using System.Collections.Generic;
using System.Threading;
using DynamicCheck.IO;
using DynamicCheck.Validation;
using Microsoft.Extensions.Logging;

namespace DynamicCheck.Testing {
    internal class TestRunner {
        private readonly TestingLifeCycle _lifeCyle;
        private readonly MessageWriter _ux;
        private readonly ITestValidator _validator;
        private readonly ILogger<TestRunner> _logger;


        public TestRunner(TestingLifeCycle lifeCycle, MessageWriter ux,  ITestValidator validator, ILogger<TestRunner> logger)
        {
            _lifeCyle = lifeCycle;
            _ux = ux;
            _validator = validator;
            _logger = logger;

            _lifeCyle.OnStageStart += RunStage;
        }

        public void RunStage(Stage stage) {
            _logger.LogDebug("Creating file for stage {name}", stage.Name);
            var file = new TestFile(stage);

            while(true) {
                if(file.Poll() && Update(stage, file)) {
                    _lifeCyle.EndStage(stage);
                    return;
                } else 
                    Thread.Sleep(100);    
            }
        }

        private bool Update(Stage stage, TestFile file) {
            Console.Clear();
            _ux.WriteFormatted($"Vooruitgang in File <DarkMagenta>{stage.FileName}.cs</> (save de file om verandering door te geven):\n\n");            

            var context = new TestContext();

            try{
                _logger.LogDebug("Recompiling {file}", file!.FilePath);
                var (tree, assembly) = AssemblyUtils.CompileStage(file.FilePath);
                context.Assembly = assembly;
                context.Root = tree.GetRoot();
                context.Type = context.Assembly.FindType(stage.TypeName);
            } catch(Exception e) {
                _ux.WriteFormatted($"   <DarkRed>Er is een error in je file: <Red>{e.Message}</>.\n");
                return false;
            }

            _logger.LogDebug("Validating tests...");
            bool result = true;
            foreach(var (test_name, test_result) in _validator.ValidateTests(stage.Tests!, context)) {
                result = result && test_result.Kind == TestResultKind.Success;
                _ux.WriteFormatted("   " + test_name + ": " + test_result.Display + "\n");
            }

            return result;
        }

        
    }
}