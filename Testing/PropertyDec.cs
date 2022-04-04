
using System;
using Newtonsoft.Json.Linq;

namespace DynamicCheck.Testing {
    internal class PropertyDec {
        public string Name { get; set; } = String.Empty;
        public JToken? Value { get; set; } = null;

        public bool ValueEquals(object value, Type type) {
            return Value!.ToObject(type)!.Equals(value);
        }

        public void Set(object instance, Type type) {
            var prop = type.FindProperty(Name);
            prop?.SetValue(instance, Value!.ToObject(prop.PropertyType));
        }
    }
    
}