using System.Text.Json.Serialization;
using API.Domain;

namespace API;

[JsonSerializable(typeof(TimeSlot[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}