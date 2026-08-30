using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.PetAggregate;
using BookyPets.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookyPets.Infrastructure.Pets.Persistence;

public class PetsRepository(BookyPetsDbContext dbContext) : IPetsRepository
{
    private readonly BookyPetsDbContext _dbContext = dbContext;

    public async Task AddPetAsync(Pet pet)
    {
        await _dbContext.Pets.AddAsync(pet);
    }

    public async Task<Pet?> GetPetAsync(Guid petId)
    {
        return await _dbContext.Pets.FindAsync(petId);
    }

    public async Task<List<Pet>> GetPetsAsync(string? search = null)
    {
        var pets = await _dbContext.Pets.ToListAsync();
        foreach (var p in pets)
        {
            Console.WriteLine($"{p.Name}: FavouriteGenre={p.FavouriteGenre}, ToString={p.FavouriteGenre?.ToString()}");
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim().ToLower();
            pets = [.. pets.Where(pet =>
                pet.Name.ToLower().Contains(searchTerm) ||
                (pet.FavouriteGenre != null && pet.FavouriteGenre.Name.ToLower().Contains(searchTerm)))];
        }

        return pets;
    }

    public Task UpdatePetAsync(Pet pet)
    {
        _dbContext.Pets.Update(pet);

        return Task.CompletedTask;
    }
}
