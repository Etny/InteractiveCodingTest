
using System.Collections.Generic;
using System.Threading;
using DynamicCheck.IO;
using DynamicCheck.Validation;

namespace DynamicCheck.Testing {
    internal class TestRunner {
        private readonly IList<Stage> _stages;
        private readonly TestingLifeCycle _lifeCyle;
        private readonly MessageWriter _ux;
        private readonly IResultWriter _result;
        private readonly ITestValidator _validator;

        private int stage_index = 0;
        private TestFile _file = null;
        public Stage CurrentStage { get => _stages[stage_index]; }

        public TestRunner(IStageProvider stageProvider, TestingLifeCycle lifeCycle, MessageWriter ux, IResultWriter result, ITestValidator validator)
        {
            _lifeCyle = lifeCycle;
            _ux = ux;
            _stages = stageProvider.GetStages();
            _result = result;
            _validator = validator;
        }

        public void Run() {
            _lifeCyle.Run();

            while(stage_index < _stages.Count) {
                if(_file == null) {
                    _lifeCyle.StartStage(CurrentStage);
                    _file = new TestFile(CurrentStage);
                }

                if(_file.Poll() && Update()) {
                    _lifeCyle.EndStage(CurrentStage);
                    stage_index++;
                    _file = null;
                } else
                    Thread.Sleep(10);    
            }

            _lifeCyle.EndTest();
            _result.WriteResult();
        }

        private bool Update() {
            Console.Clear();
            _ux.WriteFormatted($"Vooruitgang in File <DarkMagenta>{CurrentStage.FileName}.cs</> (save de file om verandering door te geven):\n\n");            

            var context = new TestContext();

            try{
                var (tree, assembly) = AssemblyUtils.CompileStage(_file.FilePath);
                context.Assembly = assembly;
                context.Root = tree.GetRoot();
                context.Type = context.Assembly.FindType(CurrentStage.TypeName);
            } catch(Exception e) {
                _ux.WriteFormatted($"   <DarkRed>Er is een error in je file: <Red>{e.Message}</>.\n");
                return false;
            }

            bool result = true;
            foreach(var (test_name, test_result) in _validator.ValidateTests(CurrentStage.Tests, context)) {
                result = result && test_result.Kind == TestResultKind.Success;
                _ux.WriteFormatted("   " + test_name + ": " + test_result.Display + "\n");
            }

            return result;
        }

        
    }
}