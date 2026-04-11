using BookyPets.Shared.SmartEnum;

namespace BookyPets.Domain.QuestAggregate;

public class QuestStatus : SmartEnum<QuestStatus>
{
    public static readonly QuestStatus Inactive = new(nameof(Inactive), 0);
    public static readonly QuestStatus Active = new(nameof(Active), 1);
    public static readonly QuestStatus Completed = new(nameof(Completed), 2);
    public static readonly QuestStatus Dropped = new(nameof(Dropped), 3);

    public QuestStatus(string name, int value) : base(name, value)
    {
    }
}
