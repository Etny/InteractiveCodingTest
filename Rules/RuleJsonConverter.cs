
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicCheck.Rules {
    internal class RuleJsonConverter : JsonConverter
    {
        private static readonly Lazy<Dictionary<string, Type>> Rules = new Lazy<Dictionary<string, Type>>(() => 
            Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.GetCustomAttribute(typeof(RuleTag)) != null)
                    .ToDictionary(
                        t => ((RuleTag)t.GetCustomAttribute(typeof(RuleTag))).Tag,
                        t => t
                    )
        );

        public override bool CanConvert(Type objectType)
            => typeof(IRule).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            if (reader.TokenType == JsonToken.Null)
                return null;

            JObject obj = JObject.Load(reader);
            IRule rule = ToRule(obj);
            serializer.Populate(obj.CreateReader(), rule);
            return rule;
        }

        private IRule ToRule(JObject obj) {
            var type = (string)obj["type"];

            if(Rules.Value.ContainsKey(type))
                return (IRule)Activator.CreateInstance(Rules.Value[type]);
            else
                return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue(((RuleTag)value.GetType().GetCustomAttribute(typeof(RuleTag))).Tag);

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