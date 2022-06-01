
using System;
using System.Collections.Generic;
using System.Reflection;
using DynamicCheck.Reflection;
using Microsoft.CodeAnalysis;

namespace DynamicCheck.Testing {
    internal record class TestContext {
        public Test CurrentTest = null;
        public Assembly Assembly { get; set; }
        public SyntaxNode Root { get; set; }
        public Type Type { get; set; }
        public MethodInfo Method { get; set; }
        public MethodInfo InstanceMethod { get; set; }
        public List<string> DebugOutput = new();
        public List<MemberDec> InstanceProperties { get; set; } = new List<MemberDec>();

        public object CreateInstance() {
            var instance = InstanceMethod != null ? 
                            new TestMethodInvoker(InstanceMethod, this).Invoke(null)
                            : Activator.CreateInstance(Type);
            
            foreach(var prop in InstanceProperties)
                prop.Set(instance, Type);

            return instance;
        }

        public TestMethodInvoker CreateInvoker() 
            => new(Method, this);

        public Type ResolveType(string name) 
            => Type.GetType(name)
            ?? Type.GetType("System." + name)
            ?? Assembly!.FindType(name);
    }
}