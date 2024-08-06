using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AppBusinessGeneric.RestClient.Models;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace AppBusinessGeneric.RestClient.Helpers;

public class DataContractHelper
{
    public T Deserialize<T>(
        Stream data,
        ContentType contentType = ContentType.Xml,
        IEnumerable<Type>? knownTypes = null)
    {
        return
            contentType == ContentType.Xml ?
            DeserializeXml<T>(data, knownTypes) :
            DeserializeJson<T>(data);
    }
    public T Deserialize<T>(
        string data,
        ContentType contentType = ContentType.Xml,
        IEnumerable<Type>? knownTypes = null)
    {
        return
            contentType == ContentType.Xml ?
            DeserializeXml<T>(data, knownTypes) :
            DeserializeJson<T>(data);
    }
    public string? Serialize<T>(
        T data,
        ContentType contentType = ContentType.Xml,
        IEnumerable<Type>? knownTypes = null)
    {
        if (typeof(T) == typeof(string))
        {
            return data as string;
        }
            return
                contentType == ContentType.Xml ?
                SerializeXml(typeof(T), data, knownTypes) :
                SerializeJson(data);
    }
    public string? Serialize<T>(
        T data,
        bool useXmlSerializer,
        ContentType contentType = ContentType.Xml,
        IEnumerable<Type>? knownTypes = null)
    {
        if (typeof(T) == typeof(string))
        {
            return data as string;
        }
        if (contentType == ContentType.Xml)
        {
            return
                useXmlSerializer ?
                SerializeXml2(typeof(T), data, knownTypes) :
                SerializeXml(typeof(T), data, knownTypes);
        }        
        return SerializeJson(data);
    }
    public T Deserialize<T>(
        string data,
        bool useXmlSerializer,
        ContentType contentType = ContentType.Xml,
        IEnumerable<Type>? knownTypes = null)
    {
        if (contentType == ContentType.Xml)
        {
            return useXmlSerializer ? DeserializeXml2<T>(data, knownTypes) : DeserializeXml<T>(data, knownTypes);
        }
        return DeserializeJson<T>(data);
    }
    private static T DeserializeXml<T>(
        Stream data,
        IEnumerable<Type>? knownTypes = null)
    {
        var serializer = new DataContractSerializer(typeof(T), knownTypes);
        return (T)(serializer.ReadObject(data) ?? new object());
    }
    private static T DeserializeXml<T>(
        string xmlData,
        IEnumerable<Type>? knownTypes = null)
    {
        var bytes = Encoding.UTF8.GetBytes(xmlData);
        using (var stream = new MemoryStream(bytes, 0, bytes.Length))
        {
            var serializer = new DataContractSerializer(typeof(T), knownTypes);
            return (T)(serializer.ReadObject(stream) ?? new object());
        }
    }
    private static T DeserializeXml2<T>(
        string xmlData,
        IEnumerable<Type>? knownTypes = null)
    {
        var bytes = Encoding.UTF8.GetBytes(xmlData);
        using var stream = new MemoryStream(bytes, 0, bytes.Length);
        Type[]? extraTypes = null;
        if (knownTypes != null)
        {
            extraTypes = knownTypes.ToArray();
        }
        var serializer = new XmlSerializer(typeof(T), extraTypes);
        return (T)(serializer.Deserialize(stream) ?? new object());
    }
    private static T DeserializeJson<T>(string jsonData)
    {
        var settings =
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            return JsonConvert.DeserializeObject<T>(jsonData, settings) ?? (T)new object();
    }
    private static T DeserializeJson<T>(Stream jsonData)
    {
        using (var sr = new StreamReader(jsonData))
        {
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<T>(reader) ?? (T)new object();
            }
        }
    }
    private static string SerializeXml(
        Type type,
        object? data,
        IEnumerable<Type>? knownTypes = null)
    {
        using (var stream = new MemoryStream())
        {
            var ser = new DataContractSerializer(type, knownTypes);
            ser.WriteObject(stream, data);
            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
    private static string SerializeXml2(
        Type type,
        object? data,
        IEnumerable<Type>? knownTypes = null)
    {
        using (var stream = new MemoryStream())
        {
            Type[]? extraTypes = null;
            if (knownTypes != null)
            {
                extraTypes = knownTypes.ToArray();
            }
            var ser = new XmlSerializer(type, extraTypes);
            var emptyNs =new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            ser.Serialize(stream, data, emptyNs);
            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
    private static string SerializeJson(object? data)
    {
        var settings =
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
        return JsonConvert.SerializeObject(data, Formatting.None, settings);
    }
}