using BookyPets.Domain.Common.Interfaces;

namespace BookyPets.Domain.BookAggregate.Events;

public record BookAcquiredEvent(Guid ReaderId, Guid BookId, Guid ProgressId) : IDomainEvent;
