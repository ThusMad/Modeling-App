using System;
using Newtonsoft.Json;
using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Converters
{
    public class VectorConverter : JsonConverter<RawVector2>
    {
        public override void WriteJson(JsonWriter writer, RawVector2 value, JsonSerializer serializer)
        {
            writer.WriteValue($"{value.X},{value.Y}");
        }

        public override RawVector2 ReadJson(JsonReader reader, Type objectType, RawVector2 existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var s = (string)reader.Value;
            var res = s.Split(',');
            return new RawVector2(float.Parse(res[0]), float.Parse(res[1]));
        }
    }
}