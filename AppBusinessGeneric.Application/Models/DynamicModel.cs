using System.Dynamic;

namespace AppBusinessGeneric.Application.Models;
public class DynamicModel : DynamicObject
{
    public object this[string propertyName]
    {
        get { return _dictionary[propertyName]; }
        set { AddProperty(propertyName, value); }
    }
    internal Dictionary<string, object> _dictionary = [];
    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        return _dictionary.TryGetValue(binder.Name, out result);
    }
    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        AddProperty(binder.Name, value);
        return true;
    }

    public void AddProperty(string name, object? value)
    {
        _dictionary[name] = value ?? new object();
    }
}