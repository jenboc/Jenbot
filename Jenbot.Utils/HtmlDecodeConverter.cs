using Newtonsoft.Json;
using System.Net;

namespace Jenbot.Utils;

public class HtmlDecodeConverter : JsonConverter<string>
{
    public override string ReadJson(JsonReader reader, Type objectType,
            string? existingValue, bool hasExistingValue, 
            JsonSerializer serializer)
    {
        var val = reader.Value as string;
        return val != null ? WebUtility.HtmlDecode(val) : "";
    }

    public override void WriteJson(JsonWriter writer, string? value, 
            JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }
}
