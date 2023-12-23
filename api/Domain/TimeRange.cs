namespace Domain;

public readonly record struct TimeRange
{
    public DateTime Start { get; }
    public DateTime End { get; }
    public TimeSpan Duration => End - Start;

    public TimeRange(DateTime start, DateTime end)
    {
        if (start >= end)
            throw new ArgumentException("Start must be before end", nameof(start));

        Start = start;
        End = end;
    }
}