namespace API;

public readonly record struct TimeRange
{
    public DateTime Start { get; }
    public DateTime End { get; }

    public TimeRange(DateTime start, DateTime end)
    {
        if (start > end)
            throw new ArgumentException("Start can't be after end", nameof(start));

        Start = start;
        End = end;
    }
}