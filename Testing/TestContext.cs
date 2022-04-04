
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace DynamicCheck.Testing {
    internal record class TestContext {
        public Assembly? Assembly { get; set; }
        public SyntaxNode? Root { get; set; }
        public Type? Type { get; set; }
        public MethodInfo? Method { get; set; }
        public MethodInfo? InstanceMethod { get; set; }
        public List<PropertyDec> InstanceProperties { get; set; } = new List<PropertyDec>();
        public object Instance {
            get {
                var instance = InstanceMethod?.InvokeWithTimeout(null!, Array.Empty<object>()) 
                            ?? Activator.CreateInstance(Type!);
                
                foreach(var prop in InstanceProperties)
                    prop.Set(instance!, Type!);

                return instance!;
            }
        }

        public Type ResolveType(string name) 
            => Type.GetType(name)
            ?? Type.GetType("System." + name)
            ?? Assembly!.FindType(name);
    }
}