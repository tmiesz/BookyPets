using BookyPets.Domain.Common.Interfaces;

namespace BookyPets.Domain.BookAggregate.Events;

public record BookReadEvent(Guid ReaderId, Guid BookId, int Pages) : IDomainEvent;
