using System.IO;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using DynamicCheck.Testing;
using Newtonsoft.Json.Serialization;

namespace DynamicCheck
{

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
           

            Console.WriteLine("Well Done!");
        }

    }
    
}