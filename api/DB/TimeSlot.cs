namespace API.DB;

using System.Collections.Generic;

internal class TimeSlot
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int MachineId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}

// get all slots for machine in time range

internal abstract class Event
{
    public int Id { get; set; }
    public ICollection<TimeSlot> TimeSlots { get; set; }
}

internal class Standstill : Event
{
    public StandstillReason Reason { get; set; }
    public int Priority { get; set; }
}

internal class StandstillReason
{
    public int Id { get; set; }

    public int? ParentId { get; set; }
    public required string Description { get; set; }

    public StandstillReason Parent { get; set; } = null!;
    public ICollection<StandstillReason> Children { get; set; } = null!;
}

internal class Machine
{
    public int Id { get; set; }
    public ICollection<TimeSlot> TimeSlots { get; set; }
}