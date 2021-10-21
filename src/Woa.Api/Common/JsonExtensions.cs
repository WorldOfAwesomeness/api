// ReSharper disable UnusedMember.Global
// ReSharper disable ConvertToAutoPropertyWhenPossible

using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace Woa.Api.Common;

public static class JsonExtensions
{
    private static readonly JsonSerializerSettings _defaultSettings = new()
    {
        Formatting = Formatting.Indented,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore,
        Error = Error,
        PreserveReferencesHandling = PreserveReferencesHandling.All,
        TypeNameHandling = TypeNameHandling.All,
        ContractResolver = new SnakeCasePropertyNamesContractResolver()
    };

    private static void Error(object? sender, ErrorEventArgs e)
    {
    }

    public static JsonSerializerSettings CurrentSettings { get; set; } = _defaultSettings;

    public static JsonSerializerSettings DefaultSettings => _defaultSettings;

    public static string ToJson(this object? source, JsonSerializerSettings? settings = default)
    {
        var toSerialize = source ?? new();

        var json = JsonConvert.SerializeObject(toSerialize, settings ?? CurrentSettings);

        return json;
    }

    public static TObject? FromJson<TObject>(
        this string? json,
        JsonSerializerSettings? settings = default)
        => json is null or ""
            ? Activator.CreateInstance<TObject>()
            : JsonConvert.DeserializeObject<TObject>(json, settings ?? CurrentSettings);

    public static object? FromJson(
        this string? json,
        JsonSerializerSettings? settings = default)
        => json is null or ""
            ? Activator.CreateInstance<object>()
            : JsonConvert.DeserializeObject(json, settings ?? CurrentSettings);

    public static object? FromJson(
        this string? json,
        string typeName,
        JsonSerializerSettings? settings = default)
    {
        if (json is null or "")
        {
            return new();
        }

        var type = Type.GetType(typeName);

        type ??= typeof(object);

        return JsonConvert.DeserializeObject(json, type, settings ?? CurrentSettings);
    }

    public static TObject WithJson<TObject>(
        this TObject? instance,
        string? json,
        JsonSerializerSettings? settings = default)
            where TObject : new()
    {
        instance ??= new();

        if (json is null or "")
        {
            return instance;
        }

        JsonConvert.PopulateObject(json, instance, settings ?? CurrentSettings);

        return instance;
    }
}
