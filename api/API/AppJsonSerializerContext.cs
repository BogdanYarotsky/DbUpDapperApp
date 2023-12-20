using System.Text.Json.Serialization;

namespace API;

[JsonSerializable(typeof(TimeSlot[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}