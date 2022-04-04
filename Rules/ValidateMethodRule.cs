
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using DynamicCheck.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicCheck.Rules {


    [RuleTag("validate_method")]
    internal class ValidateMethodRule : IRule
    {
        public IList<TruthRecord>? TruthTable { get; set; } = null;
        public JArray? In { get; set; } = null;
        public JToken? Out { get; set; } = null;

        public bool Validate(TestContext context)
        {
            var arg_types = context.Method.GetParameters().Select(p => p.ParameterType).ToArray();
            var out_type = context.Method.ReturnType;

            TruthTable ??= new List<TruthRecord> { new TruthRecord { In = In!, Out = Out! } };
            
            foreach(var truth in TruthTable) {
                var result = context.Method.InvokeWithTimeout(context.Instance, truth.ConvertedIn(arg_types));
                if(result == null || !truth.CompareOut(result, out_type))
                    return false;
            }

            return true;
        }
    
    }

    internal class TruthRecord {
        public JArray? In { get; set; }
        public JToken? Out { get; set; }
        public bool CompareOut(object result, Type returnType) {
            if(returnType.IsArray) {
                var type = returnType.GetElementType() ?? typeof(void);
                var outEnum = (IEnumerable<object>)Out!.ToObject(returnType)!;
                var resultEnum = (IEnumerable<object>)result;

                return outEnum.Zip(resultEnum, (o, r) => Convert.ChangeType(o, type).Equals(Convert.ChangeType(r, type))).All(b => b);
            }
               

            return Out!.ToObject(returnType)!.Equals(result);
        }
            
        public object[] ConvertedIn(Type[] paramTypes) 
            => paramTypes.Length > 0 ? In!.Select((item, index) => item.ToObject(paramTypes[index])!).ToArray() : Array.Empty<object>();
        
        
    }

}