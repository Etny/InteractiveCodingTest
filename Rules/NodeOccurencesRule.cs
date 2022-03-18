
using System;
using System.Linq;
using DynamicCheck.Testing;
using Microsoft.CodeAnalysis.CSharp;

namespace DynamicCheck.Rules {

    [RuleTag("node_occurences")]
    internal class NodeOccurencesRules : IRule
    {
        public SyntaxKind Kind { get; set; }
        public NodeOccurenceRestriction Restriction { get; set; } = NodeOccurenceRestriction.Exact;
        public string Content { get; set; } = "";
        public int Count { get; set; } = 1;

        public bool Validate(TestContext context)
        {
            var node = context.Root.IsolateFirst(SyntaxKind.MethodDeclaration, context.Method.Name);
            var count = node.IsolateAll(Kind, Content).Count();

            switch(Restriction) {
                case NodeOccurenceRestriction.Exact:
                    return count == Count;
            
                case NodeOccurenceRestriction.More: 
                    return count > Count;

                default: 
                case NodeOccurenceRestriction.Less:
                    return count < Count;
            }
        }
    }

    internal enum NodeOccurenceRestriction {
        Exact,
        More,
        Less
    }
}