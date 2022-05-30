
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DynamicCheck.Rules;

namespace DynamicCheck.Testing {
    internal class RuleDecJsonConverter : JsonConverter
    {
        private static readonly Lazy<Dictionary<string, Type>> Rules = new(() => 
            Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.GetCustomAttribute(typeof(RuleTag)) != null)
                    .ToDictionary(
                        t => ((RuleTag)t.GetCustomAttribute(typeof(RuleTag))!).Tag,
                        t => t
                    )
        );

        public override bool CanConvert(Type objectType)
            => typeof(RuleDec).IsAssignableFrom(objectType);


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            if (reader.TokenType == JsonToken.Null)
                return null;

            JObject obj = JObject.Load(reader);
            IRule rule = ToRule(obj) ?? throw new NullReferenceException("Failed to resolve rule declaration");
            serializer.Populate(obj.CreateReader(), rule);
            var dec = new RuleDec { Rule = rule };
            serializer.Populate(obj.CreateReader(), dec);
            return dec;
        }

        private static IRule ToRule(JObject obj) {
            string type = obj.Value<string>("type") 
                            ?? throw new ArgumentException("JObject lacks 'type' property");

            if(Rules.Value.ContainsKey(type)) { 
                return (IRule)Activator.CreateInstance(Rules.Value[type])!;
            } else
                return null;
        }

        // private string ToSnakeCase(string propName) 
        //     => char.ToLower(propName[0]) + 
        //         string.Join("", 
        //             propName.Substring(1).Select(c => 
        //                 char.IsUpper(c) ? "_" + char.ToLower(c) : c + ""
        //             )
        //         );


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue(((RuleTag)value!.GetType().GetCustomAttribute(typeof(RuleTag))!).Tag);

            foreach(var prop in value.GetType().GetProperties()) {
                writer.WritePropertyName(prop.Name);
                writer.WriteValue(prop.GetValue(value));
            }

            writer.WriteEndObject();
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class RuleTag: Attribute {
        public string Tag { get; set; }

        public RuleTag(string tag) {
            Tag = tag;
        }
    }
}