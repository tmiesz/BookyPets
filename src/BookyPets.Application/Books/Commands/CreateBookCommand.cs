using BookyPets.Application.Common.Authorization;
using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Books.Commands;

[Authorize(Roles = Role.Admin)]
public record CreateBookCommand(string Title, string Author, Genre Genre, int PageCount) : IRequest<Result<Book>>;
