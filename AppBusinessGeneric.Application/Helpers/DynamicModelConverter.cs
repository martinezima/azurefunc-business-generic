using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using AppBusinessGeneric.Application.Models;

namespace AppBusinessGeneric.Application.Helpers;



public class DynamicModelConverter : JsonConverter<DynamicModel>
{
    public override DynamicModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
    public override void Write(Utf8JsonWriter writer, DynamicModel value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        foreach (var kvp in value._dictionary)
        {
            var isGenericTypeOrDynamicModel =
                kvp.Value.GetType().IsGenericType ||
                kvp.Value.GetType() == typeof(DynamicModel);
            var resultForGeneric = "";
            if (isGenericTypeOrDynamicModel)
            {
                resultForGeneric = 
                JsonSerializer.Serialize(
                    kvp.Value,
                    new JsonSerializerOptions()
                    {
                        Converters =
                        {
                            new DynamicModelConverter()
                        }
                    });
            }
            writer.WriteString(kvp.Key, isGenericTypeOrDynamicModel ? resultForGeneric : kvp.Value.ToString());
        }
        writer.WriteEndObject();
    }
}