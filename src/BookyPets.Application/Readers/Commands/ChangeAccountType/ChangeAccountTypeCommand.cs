using BookyPets.Application.Common.Authorization;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Commands.ChangeAccountType;

[Authorize(Roles = Role.Admin)]
public record ChangeAccountTypeCommand(Guid ReaderId, AccountType AccountType):IRequest<Result<Reader>>;
