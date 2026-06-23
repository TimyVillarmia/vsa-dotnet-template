namespace Project.Infrastructure.Time;

public sealed class DateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}