using System;
using System.Dynamic;
using System.Reflection;

namespace Linearstar.MikuMikuMoving.Framework;

public class ExposedObject : DynamicObject
{
    readonly Type type;
    readonly object instance;
    
    ExposedObject(object instance)
    {
        this.instance = instance;
        type = instance.GetType();
    }
    
    public static ExposedObject? Create(object? instance) =>
        instance != null ? new ExposedObject(instance) : null;
    
    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        if (type.GetProperty(binder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) is { } property)
        {
            result = Create(property.GetValue(instance, null));
            return true;
        }
        
        if (type.GetField(binder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) is { } field)
        {
            result = Create(field.GetValue(instance));
            return true;
        }
        
        result = null;
        return false;
    }
    
    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        if (type.GetProperty(binder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) is { } property)
        {
            property.SetValue(instance, value, null);
            return true;
        }
        
        if (type.GetField(binder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) is { } field)
        {
            field.SetValue(instance, value);
            return true;
        }
        
        return false;
    }
    
    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[] args, out object? result)
    {
        if (type.GetMethod(binder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) is { } method)
        {
            result = Create(method.Invoke(instance, args));
            return true;
        }
        
        result = null;
        return false;
    }
    
    public override bool TryConvert(ConvertBinder binder, out object? result)
    {
        if (binder.Type.IsAssignableFrom(type))
        {
            result = instance;
            return true;
        }
        
        result = null;
        return false;
    }
    
    public override string ToString() =>
        instance.ToString();
}