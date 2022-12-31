using System;
using System.Dynamic;
using System.Linq;
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
        var unwrappedValue = value is ExposedObject eo ? eo.instance : value;
        
        if (type.GetProperty(binder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) is { } property)
        {
            property.SetValue(instance, unwrappedValue, null);
            return true;
        }
        
        if (type.GetField(binder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) is { } field)
        {
            field.SetValue(instance, unwrappedValue);
            return true;
        }
        
        return false;
    }
    
    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[] args, out object? result)
    {
        var unwrappedArgs = args.Select(x => x is ExposedObject eo ? eo.instance : x).ToArray();
        var method = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => x.Name == binder.Name)
            .Select(x => new
            {
                MethodInfo = x,
                Parameters = x.GetParameters(),
            })
            .Where(x => x.Parameters.Length == args.Length)
            .OrderByDescending(x => x.Parameters
                .Zip(unwrappedArgs, (p, a) => p.ParameterType.IsInstanceOfType(a))
                .Count(pa => pa))
            .Select(x => x.MethodInfo)
            .FirstOrDefault();
        
        if (method != null)
        {
            result = Create(method.Invoke(instance, unwrappedArgs));
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

    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object? result)
    {
        var defaultMemberAttribute = type.GetCustomAttribute<DefaultMemberAttribute>();
        if (defaultMemberAttribute == null)
        {
            result = null;
            return false;
        }

        var unwrappedIndexes = indexes.Select(x => x is ExposedObject eo ? eo.instance : x).ToArray();
        var indexer = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => x.Name == defaultMemberAttribute.MemberName)
            .Select(x => new
            {
                PropertyInfo = x,
                Parameters = x.GetIndexParameters(),
            })
            .OrderByDescending(x => x.Parameters
                .Zip(unwrappedIndexes, (p, i) => p.ParameterType.IsInstanceOfType(i))
                .Count(pi => pi))
            .Select(x => x.PropertyInfo)
            .FirstOrDefault();

        if (indexer != null)
        {
            result = Create(indexer.GetValue(instance, unwrappedIndexes));
            return true;
        }
        
        result = null;
        return false;
    }

    public override string ToString() =>
        instance.ToString();
}