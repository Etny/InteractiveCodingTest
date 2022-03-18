
using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DynamicCheck {
    internal static class NodeUtils {
        public static SyntaxNode IsolateFirst(this SyntaxNode parent, SyntaxKind kind, string name = "", bool match_exact = false) {
            foreach(var child in parent.ChildNodes()) {
                if(child.IsKind(kind) && (match_exact? child.MatchText(name) : child.GetText().ToString().Contains(name)))
                    return child;
            }

            foreach(var child in parent.ChildNodes()) {
                var ret = IsolateFirst(child, kind, name);
                if(ret != null)
                    return ret;
            }

            return null;
        }

        public static System.Collections.Generic.IEnumerable<SyntaxNode> IsolateAll(this SyntaxNode parent, SyntaxKind kind, string name = "", bool match_exact = false) {
            foreach(var child in parent.ChildNodes()) {
                if(child.IsKind(kind) && (match_exact? child.MatchText(name) : child.GetText().ToString().Contains(name)))
                    yield return child;
            }

            foreach(var child in parent.ChildNodes()) {
                var ret = IsolateFirst(child, kind, name);
                if(ret != null)
                    yield return ret;
            }
        }

        public static bool MatchText(this SyntaxNode node, string text) 
            => node.GetText().ToString().Trim() == text;


        public static void PrintNode(SyntaxNode node, int indent = 0) {
            Console.WriteLine(new string('\t', indent) + node.Kind() + " " + node.GetText());
            foreach(var child in node.ChildNodes())
                PrintNode(child, indent + 1);
        }
    }
}