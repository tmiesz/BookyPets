using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.PetAggregate;
using BookyPets.Infrastructure.Common.Persistence;

namespace BookyPets.Infrastructure.Pets.Persistence;

public class PetsRepository : IPetsRepository
{
    private readonly BookyPetsDbContext _dbContext;

    public PetsRepository(BookyPetsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddPetAsync(Pet pet)
    {
        await _dbContext.Pets.AddAsync(pet);
    }

    public async Task<Pet?> GetPetAsync(Guid petId)
    {
        return await _dbContext.Pets.FindAsync(petId);
    }

    public Task UpdatePetAsync(Pet pet)
    {
        _dbContext.Pets.Update(pet);

        return Task.CompletedTask;
    }
}
