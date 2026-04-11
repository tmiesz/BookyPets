using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.Common;

namespace BookyPets.Domain.QuestAggregate;

public class QuestBase : Entity
{
    public string Title { get; init; }
    public string Description { get; init; }
    public List<QuestRequirement> QuestRequirements = new List<QuestRequirement>();

    public QuestBase(string title, string description, int pages)
    {
        Title = title;
        Description = description;
    }
}
