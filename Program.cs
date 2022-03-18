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
using Newtonsoft.Json;
using System.Collections.Generic;
using DynamicCheck.Testing;
using Newtonsoft.Json.Serialization;

namespace DynamicCheck {

    class Program {

        public static void Main() {


            var json = File.ReadAllText("./Tests.json");
            var stages = JsonConvert.DeserializeObject<List<Testing.Stage>>(json, new JsonSerializerSettings() {
                ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
            
            var manager = new TestManager(stages);
            manager.Run();
            // UX.ShowStartUp();

            // Stage[] stages = {new StageA() };
            // int current_stage = 0;


            // while(current_stage < stages.Length) {
            //     if(!stages[current_stage].Begun) 
            //         stages[current_stage].Begin(); 

            //     if(stages[current_stage].Poll()) {
            //         current_stage++;
            //     } else
            //         Thread.Sleep(10);
            // }

            Console.WriteLine("Well Done!");
        }

    }
    
}