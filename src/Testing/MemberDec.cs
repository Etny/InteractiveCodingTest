using System.Reflection;
using Newtonsoft.Json.Linq;

namespace DynamicCheck.Testing;

//Quick fix, should obviously use interface with sepperate interfaces
internal class MemberDec {
    public string Name { get; set; }
    public JToken Value { get; set; }

    public bool IsProp { get; set; } = false;

    public void Set(object instance, Type type) {
        if(IsProp) {
            var prop = type.FindProperty(Name);
            prop.SetValue(instance, Value.ToObject(prop.PropertyType));
        } else {
            var field = type.FindField(Name);
            field.SetValue(instance, Value.ToObject(field.FieldType));
        }
    }


    public bool Check(object instance, Type type) {
        var typ = IsProp ? type.FindProperty(Name).PropertyType : type.FindField(Name).FieldType;
        var value = IsProp ? type.FindProperty(Name).GetValue(instance) : type.FindField(Name).GetValue(instance);
        return value != null && Value.ToObject(typ).Equals(value);
    }
}