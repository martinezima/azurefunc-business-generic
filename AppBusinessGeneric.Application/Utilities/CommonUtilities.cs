using Armanino.Integration.Utilities.Models;
using System.Globalization;
using System.Reflection;

namespace AppBusinessMedius.Utilities;
public class CommonUtilities
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
    
    /// <summary>
    /// Build url with parameters received in request.
    /// </summary>
    /// <param name="json"></param>
    /// <param name="getEndpoint"></param>
    /// <returns></returns>
    public static string BuildApiUrl(string getEndpoint, string[] filters)
    {
        string apiUrl = $"{getEndpoint}?{string.Join("&", filters)}";
        return apiUrl;
    }

    /// <summary>
    /// Build url with parameters received in request.
    /// </summary>
    /// <param name="json"></param>
    /// <param name="getEndpoint"></param>
    /// <returns></returns>
    public static string? BuildApiUrlId(string getEndpoint, List<Field> fields)
    {
        IList<string> queryParams = [];
        string? apiUrl = default;
        foreach (var kvp in fields)
        {
            if (kvp.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase))
            {
                apiUrl = $"{getEndpoint}/{kvp.Value}";
            }
        }
        return apiUrl;
    }
    
    /// <summary>
    ///  assign values to the object
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="datatest"></param>
    /// <returns></returns>
    public static T AddFieldsToObject<T>(object supplier, dynamic datatest)
    {
        foreach (var field in datatest)
        {
            SetPropValue<T>(supplier, field.Name, field.Value);
        }
        return (T)supplier;
    }
    /// <summary>
    /// Set property value of T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T? SetPropValue<T>(object obj, string name, object value)
    {
        object? retval = SetPropValue(obj, name, value);
        if (retval == null)
        {
            return default(T);
        }
        return (T)retval;
    }
    
    /// <summary>
    /// Set property value of object
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static object? SetPropValue(object obj, string name, object value)
    {
        if (name == null || obj == null)
        {
            return null;
        }
        var objinit = obj;
        foreach (string piece in name.Split('.'))
        {
            PropertyInfo? info = GetPropertyInfo(obj, piece);
            if (info != null &&info.SetMethod != null && info.GetMethod != null)
            {
                info.SetValue(obj,FormatAttributeValue(
                    value,
                    info.GetMethod.ReturnType));
            }
        }
        return objinit;
    }
    
    /// <summary>
    /// Get object lines of T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="systemObject"></param>
    /// <param name="p"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static T GetObjectLines<T>(
        string systemObject,
        PropertyInfo p,
        dynamic item)
    {
        var type = typeof(T);
        return (T)Convert.ChangeType(p.GetValue(item), type);
    }
    
    /// <summary>
    /// Get property info
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="part"></param>
    /// <returns></returns>
    private static PropertyInfo? GetPropertyInfo(object obj, string part)
    {
        PropertyInfo[] properties = obj.GetType().GetProperties();
        return properties
            .AsEnumerable()
            .FirstOrDefault(e => e.Name.ToLower(Culture)
            .Equals(part.ToLower(Culture),StringComparison.OrdinalIgnoreCase));
    }
    /// <summary>
    /// Format attribute value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private static object? FormatAttributeValue(object value, Type type)
    {
        return "" switch
        {
            string _ when type == typeof(int?) => (value is null ?
                                (int?)null :
                                Convert.ToInt32(value, Culture)),
            string _ when type == typeof(int) => Convert.ToInt32(value, Culture),
            string _ when type == typeof(long?) => (value is null ?
                                (long?)null :
                                Convert.ToInt64(value, Culture)),
            string _ when type == typeof(long) => Convert.ToInt64(value, Culture),
            string _ when type == typeof(double?) => (value is null ?
                                (double?)null :
                                Convert.ToDouble(value, Culture)),
            string _ when type == typeof(double) => Convert.ToDouble(value, Culture),
            string _ when type == typeof(decimal?) => (value is null ?
                                (decimal?)null :
                                Convert.ToDecimal(value, Culture)),
            string _ when type == typeof(decimal) => Convert.ToDecimal(value, Culture),
            string _ when type == typeof(DateTime?) => (value is null ?
                                (DateTime?)null :
                                Convert.ToDateTime(value, Culture)),
            string _ when type == typeof(DateTime) => Convert.ToDateTime(value, Culture),
            string _ when type == typeof(bool?) => (value is null ?
                                (bool?)null :
                                Convert.ToBoolean(value, Culture)),
            string _ when type == typeof(bool) => Convert.ToBoolean(value, Culture),
            string _ when type == typeof(string) => value != null ? Convert.ToString(value, Culture) : null,
            _ => null,
        };
    }
 
     
}

