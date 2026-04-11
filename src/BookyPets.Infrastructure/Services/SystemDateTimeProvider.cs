using BookyPets.Domain.Common.Interfaces;

namespace BookyPets.Infrastructure.Services;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
