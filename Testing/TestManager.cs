
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using DynamicCheck.IO;
using Microsoft.CodeAnalysis.CSharp;

namespace DynamicCheck.Testing {
    internal class TestManager {
        private readonly List<Stage> _stages;
        private int stage_index = 0;
        private TestFile _file = null;
        public Stage CurrentStage { get => _stages[stage_index]; }

        public TestManager(List<Stage> stages)
        {
            _stages = stages;
        }

        public void Run() {
            while(stage_index < _stages.Count) {
                if(_file == null)
                    _file = CurrentStage.CreateFile();
                
                if(_file.Poll() && Update()) {
                    UX.ShowStageComplete(CurrentStage.Name);
                    stage_index++;
                    _file = null;
                } else
                    Thread.Sleep(10);    

            }
        }

        private bool Update() {
            Console.Clear();
            UX.WriteFormatted($"Vooruitgang in File <DarkMagenta>{CurrentStage.FileName}.cs</> (save de file om verandering door te geven):\n\n");            

            var context = new TestContext();

            try{
                var (tree, assembly) = AssemblyUtils.CompileStage(_file.FilePath);
                context.Assembly = assembly;
                context.Root = tree.GetRoot();
                context.Type = context.Assembly.FindType(CurrentStage.TypeName);
            } catch(Exception e) {
                UX.WriteFormatted($"   <DarkRed>Er is een error in je file: <Red>{e.Message}</>.\n");
                return false;
            }

            bool result = true;
            foreach(var (test_name, test_result) in CurrentStage.ValidateTests(context)) {
                result = result && test_result.Kind == TestResultKind.Success;
                UX.WriteFormatted("   " + test_name + ": " + test_result.Display + "\n");
            }

            return result;
        }

        
    }
}