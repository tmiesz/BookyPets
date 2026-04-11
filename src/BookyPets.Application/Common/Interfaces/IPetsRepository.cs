using BookyPets.Domain.PetAggregate;

namespace BookyPets.Application.Common.Interfaces;

public interface IPetsRepository
{
    Task AddPetAsync(Pet pet);
    Task<Pet?> GetPetAsync(Guid petId);
    Task UpdatePetAsync(Pet pet);
}
