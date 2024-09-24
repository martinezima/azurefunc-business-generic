using System.Reflection;
using AppBusinessGeneric.Application.Models;
using AppBusinessGeneric.Application.Utilities;
using AppBusinessMedius.Utilities;
using Armanino.Integration.Utilities.Models;

namespace AppBusinessGeneric.Application.Helpers;

public static class ConverterObjectHelper
{
    public const string MODEL_NAMESPACE_FOR_GENERIC = "AppBusinessGeneric.Application.Models";
    public static dynamic ConvertFieldToObject(List<Field> fields, string systemObject)
    {
        if (systemObject == null)
        {
            throw new ArgumentException($"You need to provide a systemObject.");
        }
        bool isAList = CheckIfIsAList(fields,
            systemObject);
        // dynamic instance = CreateInstanceFromType(classType, isAList);
        dynamic model = CheckOverAllProperties(
            fields,
            systemObject,
            isAList);

        return model;
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

    public static dynamic CheckOverAllProperties(
        List<Field> fields,
        string systemObject,
        bool isAList)
    {
        var index = -1;
        dynamic? dynamicObject = null;
        DynamicModel? dynamicInstanceObject = null;        
        do
        {
            dynamicInstanceObject = new();
            if(isAList)
            {
                index++;
            } 
            var propertyOrObjectOrCollection = GetElementsFromFields(
                fields,
                systemObject,
                index,
                isAList);              
            List<int> indexes = [];
            for(int i = 0; i < propertyOrObjectOrCollection.Count; i++)
            {     
                if (indexes
                    .Any(index => index == i))
                {
                    continue;
                }
                var field = propertyOrObjectOrCollection.ElementAt(i);
                var propertyInfo = field.Name.Split('.');
                if (propertyInfo.Length > 1)
                {
                    var propertyName =
                        propertyInfo[0].Replace($"[{index}]", "");
                    indexes.AddRange(
                        propertyOrObjectOrCollection
                            .Select((f, index) => {
                                return f.Name.Contains(propertyName) ? index : 0;
                            })
                            .ToList());
                    dynamicInstanceObject[propertyName] =
                        FillOutPropertyObject(
                            propertyName,
                            propertyOrObjectOrCollection
                                .Where(f => f.Name.Contains(propertyName)));
                }
                else
                {
                    dynamicInstanceObject[field.Name] =
                        StringExtensions.ConverToType(field.Value);
                }              
            }
            if(isAList)
            {
                dynamicObject ??= CreateListInstance();
                dynamicObject
                .GetType()
                .GetMethod("Add")
                ?.Invoke(dynamicObject, new[] { dynamicInstanceObject });
            }
            fields =
            fields.Where( f=> isAList &&
                f.Name.Contains($"{systemObject}[",
                    StringComparison.CurrentCultureIgnoreCase))
            .ToList();            
        } while(fields.Count != 0);

        return dynamicObject ?? dynamicInstanceObject;
    }

    public static dynamic FillOutPropertyObject(
        string propertyName,
        IEnumerable<Field> allFieldItems)
    {
        var dynamicPropertyObject = ConvertFieldToObject(allFieldItems.ToList(), propertyName);
        return dynamicPropertyObject;
    }
    
    public static dynamic CreateListInstance()
    {
        //Create a generic List
        Type genericListTYpe = typeof(List<>);
        Type concreteListType = genericListTYpe.MakeGenericType(typeof(object));
        dynamic instanceList =
            Activator.CreateInstance(concreteListType) ?? new object();
        return instanceList;
    }

    public static ICollection<Field> GetElementsFromFields(
        List<Field> fields,
        string systemObject,
        int index, 
        bool isAList)
    {
        var search =  isAList ? $"{systemObject}[{index}]." : $"{systemObject}.";
        return fields
                .Where(f =>
                           f.Name.Contains(search,
                               StringComparison.CurrentCultureIgnoreCase))
                .Select(f => {
                    f.Name = f.Name.Replace(search, "");
                    return f;
                }).ToList();
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


}