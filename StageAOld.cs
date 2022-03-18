// using System;
// using System.Linq;
// using System.Collections.Generic;
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CSharp;
// using DynamicCheck;
// using System.Reflection;
// using System.IO;

// namespace DynamicCheck.Stages {

//     using TestList = List<(string, Func<bool>)>;
//     internal class StageA : Stage {

//         public StageA(): base("StageA", "OpdrachtA" ) {
//             // Tests = new TestList { 
//             //     ("Opgave 1", CheckNumberDef),
//             //     ("Opgave 2", CheckProductFunction),
//             //     ("Opgave 3", CheckIfElseFunction),
//             //     ("Opgave 4", CheckForLoopFunction),
//             //     ("Opgave 5", CheckStringArrayFunction),
//             //     ("Opgave 6", CheckCarFunction)
//             // };
//         }

//         [TestAtribute("Opgave 1")]
//         protected bool CheckNumberDef() {
//             var node = Root.IsolateFirst(SyntaxKind.MethodDeclaration, "void Opgave1")
//                         ?? throw new Exception("Kan functie 'Opgave1' niet vinden.");
                        
//             var dec = node.IsolateFirst(SyntaxKind.LocalDeclarationStatement);
//             var name = dec?.IsolateFirst(SyntaxKind.VariableDeclarator, "number", true);

//             var type = dec?.IsolateFirst(SyntaxKind.IdentifierName, "var") 
//                         ?? dec?.IsolateFirst(SyntaxKind.PredefinedType, "int");

//             var val = dec?.IsolateFirst(SyntaxKind.NumericLiteralExpression, "50", true) 
//                         ?? Root?.IsolateFirst(SyntaxKind.NumericLiteralExpression, "50", true);
            
//             return !(dec == null || name == null || type == null || val == null);
//         }

//         [TestAtribute("Opgave 2")]
//         protected bool CheckProductFunction()
//             => Type.FindAndValidateFunction("Opgave2", new Type[] { typeof(int), typeof(int) }, 
//                 (2_856_904, new object[] {362, 7892}),
//                 (-219_492, new object[] {-234, 938})
//             );

//         [TestAtribute("Opgave 3")]
//         protected bool CheckIfElseFunction() {
//             var method = Type.FindMethod("Opgave3", typeof(int));

//             return AssemblyUtils.ValidateFunction(method, 
//                         (a, b) => a?.Trim().ToLower() == b?.Trim().ToLower(),
//                         ("goedemorgen", new object[]{ 7 }),
//                         ("goedemiddag", new object[]{ 17 }),
//                         ("goedemiddag", new object[]{ 12 })
//                 );
//         }

//         [TestAtribute("Opgave 4")]
//         protected bool CheckForLoop, Type[] method_argsFunction() {
//             var method = Type.FindMethod("Opgave4", typeof(StringWriter));

//             var writer = new StringWriter();
//             method.Invoke(null, new object[]{ writer });

//             var method_node = Root.IsolateFirst(SyntaxKind.MethodDeclaration, "Opgave4");
//             var write_count = method_node.IsolateAll(SyntaxKind.InvocationExpression, ".Write").Count();
//             var string_expr = method_node.IsolateFirst(SyntaxKind.StringLiteralExpression);

//             return writer.ToString() == "12345" 
//                 && write_count == 1 
//                 && string_expr == null;
//         }

//         [TestAtribute("Opgave 5")]
//         protected bool CheckStringArrayFunction() {
//             var method = Type.FindMethod("Opgave5");

//             var result = (string[]) method.Invoke(null, Array.Empty<object>());

//             return result != null
//                 && (new string[] { "aardbeien", "peren", "appels" }).SequenceEqual(result);
//         }

//         [TestAtribute("Opgave 6")]
//         protected  bool CheckCarFunction() {
//             var car_type = Assembly.FindType("Car");
//             var method = Type.FindMethod("Opgave6");            
            
//             var brand_prop = car_type.GetProperties().Where(t => t.Name == "Brand").FirstOrDefault()
//                         ?? throw new Exception("Kan property 'Brand' niet vinden in class 'Car'. Heb je hem weggegooid?");


//             var result = method.Invoke(null, Array.Empty<object>());

//             return result != null && ((string)brand_prop.GetValue(result, null)).ToLower() == "peugot";
//         }
//     }
// }