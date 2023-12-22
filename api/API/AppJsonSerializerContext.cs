using System.Text.Json.Serialization;
using API.Domain;
using TimeSlot = API.Db.TimeSlot;

namespace API;

[JsonSerializable(typeof(TimeSlot[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}