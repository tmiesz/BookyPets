using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.Common.Interfaces;

namespace BookyPets.Domain.SessionAggregate.Events;

public sealed record SessionCompletedEvent(
    Guid ReaderId,
    Guid? PetId,
    Guid ProgressId,
    Guid BookId,
    Genre Genre,
    int PagesRead,
    int MinutesRead
) : IDomainEvent;
