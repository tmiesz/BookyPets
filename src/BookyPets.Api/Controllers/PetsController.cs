using BookyPets.Api.Common;
using BookyPets.Application.Common.Authorization;
using BookyPets.Application.Pets.Commands.CreatePet;
using BookyPets.Application.Pets.Commands.GiveExperience;
using BookyPets.Application.Pets.Queries.GetPet;
using BookyPets.Application.Pets.Queries.SearchPets;
using BookyPets.Contracts.Pets;
using BookyPets.Shared.Mediator.Abstractions;
using Microsoft.AspNetCore.Mvc;
using DomainGenre = BookyPets.Domain.BookAggregate.Genre;
using DomainSpecies = BookyPets.Domain.PetAggregate.Species;

namespace BookyPets.Api.Controllers;

[Authorize]
[Route("[controller]")]
public class PetsController(IMediator _mediator) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreatePet(CreatePetRequest request)
    {
        DomainGenre? genre = null;
        DomainSpecies? species = null;

        if (request.FavouriteGenre.HasValue)
        {
            if (!DtoConverter.TryToDomain(request.FavouriteGenre.Value, out var domainGenre))
            {
                return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Invalid genre type");
            }

            genre = domainGenre;
        }


        if (!DtoConverter.TryToDomain(request.Species, out var domainSpecies))
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Invalid species type");
        }

        species = domainSpecies;

        var command = new CreatePetCommand(request.Name, species, genre);

        var createPetResult = await _mediator.SendAsync(command);

        return createPetResult.Match(
            pet => Ok(new PetResponse(
                pet.Id,
                pet.Name,
                DtoConverter.ToDto(pet.Species),
                IconResolver.Species.Resolve(species),
                pet.FavouriteGenre is not null ? DtoConverter.ToDto(pet.FavouriteGenre) : null,
                pet.Level)),
            Problem
        );
    }

    [HttpGet("{petId:guid}")]
    public async Task<IActionResult> GetPet(Guid petId)
    {
        var query = new GetPetQuery(petId);

        var getPetResult = await _mediator.SendAsync(query);

        return getPetResult.Match(
            pet => Ok(new PetResponse(
                pet.Id,
                pet.Name,
                DtoConverter.ToDto(pet.Species),
                IconResolver.Species.Resolve(pet.Species),
                pet.FavouriteGenre is not null ? DtoConverter.ToDto(pet.FavouriteGenre) : null,
                pet.Level)),
            Problem
        );
    }

    [HttpGet("")]
    public async Task<IActionResult> SearchPets([FromQuery] string? search)
    {
        var query = new SearchPetsQuery(search);

        var getPetsResult = await _mediator.SendAsync(query);

        return getPetsResult.Match(
            pets => Ok(pets.Select(pet => new PetResponse(
                pet.Id,
                pet.Name,
                DtoConverter.ToDto(pet.Species),
                IconResolver.Species.Resolve(pet.Species),
                pet.FavouriteGenre is not null ? DtoConverter.ToDto(pet.FavouriteGenre) : null,
                pet.Level))),
            Problem
        );
    }

    [HttpPost("{petId:guid}/experience/{experience:int}")]
    public async Task<IActionResult> GiveExperience(Guid petId, int experience)
    {
        var command = new GiveExperienceCommand(petId, experience);

        var giveExperienceResult = await _mediator.SendAsync(command);

        return giveExperienceResult.Match(
            NoContent,
            Problem);
    }
}
