
using System;
using System.Collections.Generic;
using DynamicCheck.Testing;
using Newtonsoft.Json.Linq;

namespace DynamicCheck.Rules {

    [RuleTag("validate_custom_type")]
    internal class ValidateCustomTypeRules : IRule
    {
        public string TypeName { get; set; }
        public IList<PropertyDec> Props { get; set; }

        public bool Validate(TestContext context)
        {
            var type = context.Assembly.FindType(TypeName);

            if(context.Method.ReturnType != type)
                return false;

            var result = context.Method.Invoke(null, Array.Empty<object>());  
            
            if(result == null)
                return false;

            foreach(var prop in Props) {
                var backing_prop = type.FindProperty(prop.Name);
                if(!prop.Compare(backing_prop.GetValue(result), backing_prop.PropertyType))
                    return false;
            }

            return true;
        }
    }

    internal class PropertyDec {
        public string Name { get; set; }
        public JToken Value { get; set; }

        public bool Compare(object value, Type type) {
            return Value.ToObject(type).Equals(value);
        }
    }
}