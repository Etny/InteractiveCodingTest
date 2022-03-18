
using System;
using System.Linq;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;

namespace DynamicCheck {
    internal static class AssemblyUtils {
        public static Assembly CompileStage(string filePath) {
           var provider = new CSharpCodeProvider();
           var parameters = new CompilerParameters {
               GenerateInMemory = true
           };

            var results = provider.CompileAssemblyFromSource(parameters, File.ReadAllText(filePath));

           return results.Errors.HasErrors ? null : results.CompiledAssembly;
        }

        public static bool ValidateFunction<T>(MethodInfo method, Func<T, T, bool> check, params (T, object[])[] args) {
            foreach(var p in args) {
                var result = (T)method.Invoke(null, p.Item2);
                if(!check(p.Item1, result))
                    return false;
            }
            return true;
        }

        public static bool ValidateFunction<T>(MethodInfo method,  params (T, object[])[] args) 
            where T: IEquatable<T> 
            => ValidateFunction(method, (a, b) => a.Equals(b), args);

        public static bool FindAndValidateFunction<T>(this Type type, string name, Type[] method_args, Func<T, T, bool> check, params (T, object[])[] args)
            =>  ValidateFunction(type.FindMethod(name, method_args), check, args);

        public static bool FindAndValidateFunction<T>(this Type type, string name, Type[] method_args, params (T, object[])[] args)
            where T: IEquatable<T>
            =>  ValidateFunction(type.FindMethod(name, method_args), args);
        
        public static MethodInfo FindMethod(this Type type, string name, params Type[] args)
            => type.GetMethod(name, args)
                ?? throw new Exception($"Kan method {name} niet vinden in class {type.Name}. Heb je hem weggegooid?");

        public static Type FindType(this Assembly assembly, string name)
            => assembly.GetTypes().Where(t => t.Name == name).FirstOrDefault()
                ?? throw new Exception($"Kan class {name} niet vinden. Heb je hem weggegooid?");
    }
}