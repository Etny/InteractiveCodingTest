
using System;
using System.Linq;
using System.Collections.Generic;
using DynamicCheck.Testing;
using Newtonsoft.Json.Linq;

namespace DynamicCheck.Rules {

    [RuleTag("validate_custom_type")]
    internal class ValidateCustomTypeRules : IRule
    {
        public string TypeName { get; set; } = String.Empty;
        public IList<PropertyDec> Props { get; set; } = new List<PropertyDec>();

        public bool Validate(TestContext context)
        {
            var type = context.Assembly!.FindType(TypeName);

            var instance = context.Instance;
            var invoke_result = context.CreateInvoker().Invoke(instance);
            
            object result = context.Method.ReturnType == type 
                            ?   invoke_result
                            :   instance;
            
            if(result == null)
                return false;

            foreach(var prop in Props) {
                var backing_prop = type.FindProperty(prop.Name);
                var backing_value = backing_prop.GetValue(result);
                if(backing_value == null) return false;
                if(!prop.ValueEquals(backing_value, backing_prop.PropertyType))
                    return false;
            }

            return true;
        }
    }
}