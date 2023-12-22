namespace API.DB;

using System.Collections.Generic;

public class TimeSlot
{
    public int Id { get; set; }

    public int EventId { get; set; }
    public int MachineId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public Event Event { get; set; } = null!;
    public Machine Machine { get; set; } = null!;
}

// get all slots for machine in time range

public abstract class Event
{
    public int Id { get; set; }
    public ICollection<TimeSlot> TimeSlots { get; set; }
}

public class Standstill : Event
{
    public StandstillReason Reason { get; set; }
    public int Priority { get; set; }
}

public class StandstillReason
{
    public int Id { get; set; }

    public int? ParentId { get; set; }
    public required string Description { get; set; }

    public StandstillReason Parent { get; set; } = null!;
    public ICollection<StandstillReason> Children { get; set; } = null!;
}

public class Machine
{
    public int Id { get; set; }
    public ICollection<TimeSlot> TimeSlots { get; set; }
}