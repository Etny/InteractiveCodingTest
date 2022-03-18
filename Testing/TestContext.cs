
using System;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace DynamicCheck.Testing {
    internal class TestContext {
        public Assembly Assembly { get; set; }
        public SyntaxNode Root { get; set; }
        public Type Type { get; set; }
        public MethodInfo Method { get; set; }

        public Type ResolveType(string name) 
            => Type.GetType(name)
            ?? Type.GetType("System." + name)
            ?? Assembly.FindType(name);
    }
}