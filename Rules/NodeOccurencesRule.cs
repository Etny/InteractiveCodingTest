
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
            var node = context.Root!.IsolateFirst(SyntaxKind.MethodDeclaration, context.Method!.Name);
            var count = node!.IsolateAll(Kind, Content).Count();

            return Restriction switch
            {
                NodeOccurenceRestriction.Exact => count == Count,
                NodeOccurenceRestriction.More => count > Count,
                _ => count < Count,
            };
        }
    }

    internal enum NodeOccurenceRestriction {
        Exact,
        More,
        Less
    }
}