using System.IO;
using System.Linq;
using System.CodeDom;
using Microsoft.CSharp;
using System;
using System.Threading;
using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using DynamicCheck.Stages;
using System.Resources;
using System.Reflection;

namespace DynamicCheck {

    class Program {

        public static void Main() {

            // foreach(var s in Assembly.GetExecutingAssembly().GetManifestResourceNames()) Console.WriteLine(s);
            // var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DynamicCheck.TestCode.test.txt");
            // if(stream == null) 
            //     Console.WriteLine("NOPO");
            // using (var reader = new StreamReader(stream))
            //     Console.WriteLine(reader.ReadToEnd());

            UX.ShowStartUp();

            Stage[] stages = {new StageA() };
            int current_stage = 0;


            while(current_stage < stages.Length) {
                if(!stages[current_stage].Begun) 
                    stages[current_stage].Begin(); 

                if(stages[current_stage].Poll()) {
                    current_stage++;
                } else
                    Thread.Sleep(10);
            }

            Console.WriteLine("Well Done!");
        }

    }
    
}