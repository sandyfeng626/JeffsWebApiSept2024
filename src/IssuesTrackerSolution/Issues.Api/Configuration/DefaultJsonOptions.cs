using System.Text.Json;
using System.Text.Json.Serialization;

namespace HtTemplate.Configuration;
public static class DefaultJsonOptions
{
    public static void Configure(JsonOptions options)
    {
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    }
}