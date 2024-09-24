using System.Reflection;
using AppBusinessMedius.Utilities;
using Armanino.Integration.Utilities.Models;

namespace AppBusinessGeneric.Application.Helpers;

public static class ConverterBlobStorageItemHelper
{
    public const string MODEL_NAMESPACE_FOR_GENERIC = "AppBusinessGeneric.Application.Models";
    public static dynamic ConvertFieldToObject(List<Field> fields, string systemObject)
    {
        if (systemObject == null)
        {
            throw new ArgumentException($"You need to provide a systemObject.");
        }
        Type classType = GetTypeBySystemObject(systemObject);
        bool isAList = CheckIfIsAList(fields,
            systemObject);
        dynamic instance = CreateInstanceFromType(classType, isAList);
        CheckOverAllProperties(
            fields,
            instance,
            classType,
            systemObject,
            isAList);

        return instance;
    }
    public static Type GetTypeBySystemObject(string systemObject)
    {
        var type = Type.GetType($"{MODEL_NAMESPACE_FOR_GENERIC}.{systemObject}");
        if (type == null)
        {
            throw new NullReferenceException($"The type {systemObject} does not exist.");
        }
        return type;
    }
    
    public static dynamic CreateInstanceFromType(Type classType, bool isAList)
    {
        dynamic instance;
        if (isAList)
        {
            Type genericListType = typeof(List<>);
            Type concreteListType = genericListType.MakeGenericType(classType);
            instance = Activator.CreateInstance(concreteListType) ?? new object();
        }
        else
        {
            instance = Activator.CreateInstance(classType) ?? new object();
        }        
        return instance;
    }

    public static bool CheckIfIsAList(
        List<Field> fields,
        string systemObject)
    {
        return fields
                .Where(f =>
                           f.Name.Contains($"{systemObject}[",
                               StringComparison.CurrentCultureIgnoreCase))
                .Any();
    }

    public static ICollection<Field> GetElementsFromFields(
        List<Field> fields,
        string systemObject,
        int index, 
        bool isAList)
    {
        var search =  isAList ? $"{systemObject}[{index}]" : $"{systemObject}.";
        return fields
                .Where(f =>
                           f.Name.Contains(search,
                               StringComparison.CurrentCultureIgnoreCase))
                .Select(f => {
                    f.Name = f.Name.Replace(search, "");
                    return f;
                }).ToList();
    }

    public static dynamic CheckOverAllProperties(
        List<Field> fields,
        dynamic instance,
        Type classType,
        string systemObject,
        bool isAList)
    {
        var index = -1;
        object? itemInstance = null;
        do
        {            
            if(isAList)
            {
                index++;
                itemInstance = Activator.CreateInstance(classType) ?? new object();
            }
            var data = GetElementsFromFields(fields, systemObject, index, isAList);
            PropertyInfo[] properties = classType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var fieldItem =
                   data
                   .Where(f => f.Name.Contains(property.Name,
                        StringComparison.CurrentCultureIgnoreCase));
                var typeCode = Type.GetTypeCode(property.PropertyType);
                FillOutObjectFromListFields(
                isAList ? itemInstance : instance,
                typeCode,
                fieldItem,
                property);
            }            
            if(isAList)
            {
                instance
                .GetType()
                .GetMethod("Add")
                .Invoke(instance, new[] { itemInstance });
            }
            fields =
            fields.Where( f=> !isAList ||
                f.Name.Contains($"{systemObject}[",
                    StringComparison.CurrentCultureIgnoreCase))
            .ToList();
        } while(fields.Count != 0);

        return instance;
    }

    public static void FillOutObjectFromListFields(
        dynamic instance,
        TypeCode typeCode,
        IEnumerable<Field> fieldItem,
        PropertyInfo property)
    {
        switch (typeCode)
        {
            case TypeCode.Char:
            case TypeCode.Boolean:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
            case TypeCode.String:
                if(fieldItem.Any())
                {                    
                    CommonUtilities.SetPropValue(
                        instance,
                        property.Name,
                        fieldItem.ElementAt(0).Value);
                }
            break;
            case TypeCode.Object:
                if(fieldItem.Any())
                {
                    var systemObject = GetNameOfPropertyType(property.PropertyType);
                    var newListField = ReplacePropertyNameWithClassName(
                        fieldItem,
                        property.Name,
                        systemObject);
                    var propertyInstance = ConvertFieldToObject(
                        newListField,
                        systemObject);
                    property.SetValue(instance, propertyInstance);                    
                }
            break;
            case TypeCode.Byte:
            break;
        }  
    }

    public static string GetNameOfPropertyType(Type propertyType)
    {
        if (typeof(IEnumerable<object>).IsAssignableFrom(propertyType)  &&
            propertyType != typeof(string))
        {
            return propertyType.GetTypeInfo().GenericTypeArguments[0].Name;
        }
        else
        {
            return propertyType.GetTypeInfo().Name;
        }
    }

    public static List<Field> ReplacePropertyNameWithClassName(
        IEnumerable<Field> fieldItem,
        string oldValue,
        string newValue)
    {
        return fieldItem.Select(f =>
        {
            f.Name = f.Name.Replace($"{oldValue}[", $"{newValue}[");
            return f;
        }).ToList();
    }
}