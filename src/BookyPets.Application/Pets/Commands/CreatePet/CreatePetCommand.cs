using BookyPets.Application.Common.Authorization;
using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.PetAggregate;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Pets.Commands.CreatePet;

[Authorize(Roles = Role.Admin)]
public record CreatePetCommand(string Name, Genre? Genre) : IRequest<Result<Pet>>;
