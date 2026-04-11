using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.SessionAggregate.Events;

namespace BookyPets.Domain.QuestAggregate;

public abstract class QuestRequirement(int target)
{
    public int Target => target;
    public abstract int ExtractProgress(SessionCompletedEvent e);
    public bool IsCompleted(int progress) => progress >= Target;

    public sealed class ReadPages(int target) : QuestRequirement(target)
    {
        public override int ExtractProgress(SessionCompletedEvent e) => e.PagesRead;
    }

    public sealed class CompleteSessions(int target) : QuestRequirement(target)
    {
        public override int ExtractProgress(SessionCompletedEvent e) => 1;
    }

    public sealed class ReadMinutes(int target) : QuestRequirement(target)
    {
        public override int ExtractProgress(SessionCompletedEvent e) => e.MinutesRead;
    }

    // public sealed class FinishBook(Guid bookId) : QuestRequirement(1)
    // {
    //     public override int ExtractProgress(SessionCompletedEvent e) =>
    //         e.BookId == bookId && e.BookFinished ? 1 : 0;
    // }

    // public sealed class FinishGenre(Genre genre) : QuestRequirement(1)
    // {
    //     public override int ExtractProgress(SessionCompletedEvent e) =>
    //         e.Genre == genre && e.BookFinished ? 1 : 0;
    // }
}
