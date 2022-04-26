
using System;
using System.Collections.Generic;
using System.Reflection;
using DynamicCheck.Reflection;
using Microsoft.CodeAnalysis;

namespace DynamicCheck.Testing {
    internal record class TestContext {
        public Assembly Assembly { get; set; }
        public SyntaxNode Root { get; set; }
        public Type Type { get; set; }
        public MethodInfo Method { get; set; }
        public MethodInfo InstanceMethod { get; set; }
        public Dictionary<string, List<string>> DebugOutput = new();
        public List<PropertyDec> InstanceProperties { get; set; } = new List<PropertyDec>();
        public object Instance {
            get {
               
                var instance = InstanceMethod != null ? 
                                new TestMethodInvoker(InstanceMethod, this).Invoke(null)
                                : Activator.CreateInstance(Type);
                
                foreach(var prop in InstanceProperties)
                    prop.Set(instance, Type);

                return instance;
            }
        }

        public TestMethodInvoker CreateInvoker() 
            => new(Method, this);

        public void AddDebugOutput(string funcName, IEnumerable<string> output) {
            if (!DebugOutput.TryGetValue(funcName, out List<string> debug))
            {
                debug = new();
                DebugOutput.Add(funcName, debug);
            }

            debug.AddRange(output);
        }

        public List<string> ExtractDebugOutput() {
            var list = new List<string>();
            
            foreach(var func in DebugOutput.Keys) {
                list.Add(func + ": ");
                foreach(var line in DebugOutput[func])
                    list.Add("\t" + line);    
            }

            DebugOutput.Clear();
            return list;
        }

        public Type ResolveType(string name) 
            => Type.GetType(name)
            ?? Type.GetType("System." + name)
            ?? Assembly!.FindType(name);
    }
}