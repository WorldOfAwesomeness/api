using Newtonsoft.Json;

namespace Woa.Api.Common
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerSettings _defaultSettings = new()
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };

        private static JsonSerializerSettings _currentSettings = _defaultSettings;

        public static JsonSerializerSettings CurrentSettings
        {
            get => _currentSettings;
            set => _currentSettings = value;
        }

        public static JsonSerializerSettings DefaultSettings => _defaultSettings;

        public static string ToJson(this object? source, JsonSerializerSettings? settings = default)
        {
            var toSerialize = source ?? new ();

            var json = JsonConvert.SerializeObject(toSerialize, settings ?? CurrentSettings);

            return json;
        }

        public static TObject? FromJson<TObject>(
            this string? json,
            JsonSerializerSettings? settings = default)
            where TObject : new()
        {
            if (json is null or "")
            {
                return new();
            }

            return JsonConvert.DeserializeObject<TObject>(json, settings ?? CurrentSettings);
        }

        public static object? FromJson(
            this string? json,
            JsonSerializerSettings? settings = default)
        {
            if (json is null or "")
            {
                return new();
            }

            return JsonConvert.DeserializeObject(json, settings ?? CurrentSettings);
        }

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

        public static TObject WithJson<TObject>(this TObject? instance, string? json, JsonSerializerSettings? settings = default)
            where TObject : new()
        {
            instance ??= new ();

            if (json is null or "")
            {
                return instance;
            }

            JsonConvert.PopulateObject(json, instance, settings ?? CurrentSettings);

            return instance;
        }
    }
}