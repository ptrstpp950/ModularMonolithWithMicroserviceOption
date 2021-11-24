using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bootloader.Contracts
{
    public static class BootloaderJsonSerializerDefaults
    {
        public static JsonSerializerSettings DefaultSerializerSettings
        {
            get
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                };
                settings.Converters.Add(new StringEnumConverter
                {
                    CamelCaseText = false
                });
                return settings;
            }
        }
        
        private static readonly ConcurrentDictionary<string, Type> TypesMap = new ConcurrentDictionary<string, Type>();
        private static JsonSerializerSettings _serializerSettings = DefaultSerializerSettings;

        public static string Serialize<T>(T @object) =>
            Serialize<T>(@object, _serializerSettings);
        
        public static string Serialize<T>(T @object, JsonSerializerSettings serializerSettings) =>
            JsonConvert.SerializeObject(@object, serializerSettings);

        public static T Deserialize<T>(string json) =>
            Deserialize<T>(json, _serializerSettings);
        
        public static T Deserialize<T>(string json, JsonSerializerSettings serializerSettings) =>
            JsonConvert.DeserializeObject<T>(json, serializerSettings);

        public static T Deserialize<T>(string json, string type) =>
            Deserialize<T>(json, type, _serializerSettings);
        
        public static T Deserialize<T>(string json, string type, JsonSerializerSettings serializerSettings)
        {
            if (TypesMap.ContainsKey(type))
            {
                return (T)JsonConvert.DeserializeObject(json, TypesMap[type], serializerSettings);
            }
            throw new JsonSerializationException($"Unknown type: {type}");
        }

        public static void RegisterType<T>() => RegisterType(typeof(T));
        public static void RegisterType(Type type) => RegisterType(type, type.Name);
        public static void RegisterType(Type type, string name) => TypesMap[name] = type;

        public static void OverrideDefaultSerializerSettings(JsonSerializerSettings settings) =>
            _serializerSettings = settings;
    }
}