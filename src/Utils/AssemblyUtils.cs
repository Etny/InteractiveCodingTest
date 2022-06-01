
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace DynamicCheck {
    internal static class AssemblyUtils {
        
        public static PropertyInfo FindProperty(this Type type, string name/*, params Type[] args*/)
            => type.GetProperties().FirstOrDefault(p => p.Name == name)
                ?? throw new Exception($"Kan property {name} niet vinden in class {type.Name}. Heb je hem weggegooid?");
        
        public static FieldInfo FindField(this Type type, string name/*, params Type[] args*/)
            => type.GetFields().FirstOrDefault(p => p.Name == name)
                ?? throw new Exception($"Kan property {name} niet vinden in class {type.Name}. Heb je hem weggegooid?");

        public static MethodInfo FindMethod(this Type type, string name/*, params Type[] args*/)
            => type.GetMethods().FirstOrDefault(m => m.Name == name)
                ?? throw new Exception($"Kan method {name} niet vinden in class {type.Name}. Heb je hem weggegooid?");

        public static Type FindType(this Assembly assembly, string name)
            => assembly.GetTypes().Where(t => t.Name == name).FirstOrDefault()
                ?? throw new Exception($"Kan class {name} niet vinden. Heb je hem weggegooid?");
        
    }
}