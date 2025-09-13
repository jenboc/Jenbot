using Newtonsoft.Json;
using System.Net;

namespace Jenbot.Utils;

public class HtmlDecodeArrayConverter : JsonConverter<string[]>
{
    public override string[] ReadJson(JsonReader reader, Type objectType,
            string[]? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
    {
        var arr = serializer.Deserialize<string[]>(reader);
        if (arr is null)
            return [];

        // This won't be null since arr is not null
        return arr.Select(WebUtility.HtmlDecode).ToArray()!;
    }

    public override void WriteJson(JsonWriter writer, string[]? value,
            JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}
