using System;
using Newtonsoft.Json;
using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Converters
{
    public class ColorConverter : JsonConverter<RawColor4>
    {
        public override void WriteJson(JsonWriter writer, RawColor4 value, JsonSerializer serializer)
        {
            writer.WriteValue($"{value.R},{value.G},{value.B},{value.A}");
        }

        public override RawColor4 ReadJson(JsonReader reader, Type objectType, RawColor4 existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var s = (string)reader.Value;
            var res = s.Split(',');
            return new RawColor4(float.Parse(res[0]), float.Parse(res[1]), float.Parse(res[2]), float.Parse(res[3]));
        }
    }
}