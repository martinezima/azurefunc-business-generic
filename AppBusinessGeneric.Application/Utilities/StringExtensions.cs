using System.Reflection;

namespace AppBusinessGeneric.Application.Utilities;

public static class StringExtensions
{
    public static dynamic ConverToType(this string value)
    {
        foreach (Type type in ConvertibleTypes)
        {
            var obj = Activator.CreateInstance(type) ?? new object();
            var methodParamterTypes = new Type[]
            {
                typeof(string), 
                type.MakeByRefType()
            };
            var method = type.GetMethod("TryParse", methodParamterTypes);
            var methodParameters = new object[] { value, obj };
            object result = method?.Invoke(null, methodParameters) ?? new bool();
            bool success = (bool)result;
            if (success)
            {
                return methodParameters[1];
            }
        }
        return value;
    }
    private static Type[]? _convertibleTypes;
    private static Type[] ConvertibleTypes
    {
        get
        {
            if( _convertibleTypes == null )
            {
                _convertibleTypes =
                [
                    // typeof(System.SByte),
                    // typeof(System.Byte),
                    typeof(System.Int16),
                    typeof(System.UInt16),
                    typeof(System.Int32),
                    typeof(System.UInt32),
                    typeof(System.Int64),
                    typeof(System.UInt64),
                    // typeof(System.Single),
                    typeof(System.Double),
                    typeof(System.Decimal),
                    typeof(System.DateTime),
                    typeof(System.Guid),
                    typeof(System.Boolean)
                ];
            }
            return _convertibleTypes;
        }
    }
}