using BookyPets.Domain.Common.Interfaces;

namespace BookyPets.Domain.BookAggregate.Events;

public record PageReadEvent(Guid ReaderId, Guid BookId, int Page) : IDomainEvent;
