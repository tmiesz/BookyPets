using BookyPets.Shared.SmartEnum;

namespace BookyPets.Domain.SessionAggregate;

public class SessionStatus : SmartEnum<SessionStatus>
{
    public static readonly SessionStatus Active = new(nameof(Active), 0);
    public static readonly SessionStatus Completed = new(nameof(Completed), 1);
    public static readonly SessionStatus Dropped = new(nameof(Dropped), 2);

    public SessionStatus(string name, int value) : base(name, value)
    {
    }
}
