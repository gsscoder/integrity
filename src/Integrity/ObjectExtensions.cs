using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ObjectExtensions
{
    public static IEnumerable<T> PropertiesOf<T>(this object obj) => (
        from property in GetProperties(obj.GetType())
        where property.PropertyType.Equals(typeof(T))
        select property.GetValue(obj)
        ).Cast<T>();

    static PropertyInfo[] GetProperties(Type type) => type.GetProperties(BindingFlags.Public);
}
