using BookyPets.Domain.Common.Interfaces;

namespace BookyPets.Domain.Tests.TestUtils;

public class FakeDateTimeProvider : IDateTimeProvider
{
    private DateTime _current = DateTime.UtcNow;

    public DateTime UtcNow => _current;

    public void Advance(TimeSpan timeSpan)
    {
        _current = _current.Add(timeSpan);
    }
}
