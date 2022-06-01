
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
        public IList<MemberDec> Members { get; set; } = new List<MemberDec>();

        public bool Validate(TestContext context)
        {
            var type = context.Assembly!.FindType(TypeName);
            var instance = context.CreateInstance();

            var invoke_result = context.CreateInvoker().Invoke(instance);
            
            object result = context.Method.ReturnType == type 
                            ?   invoke_result
                            :   instance;
            
            if(result == null)
                return false;

            foreach(var prop in Members) 
                if(!prop.Check(result, type)) 
                    return false;
            
            return true;
        }
    }
}