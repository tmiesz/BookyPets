using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
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

    public async Task<List<Pet>> GetPetsAsync(List<Guid> petIds)
    {
        return await _dbContext.Pets
            .Where(p => petIds.Contains(p.Id))
            .ToListAsync();
    }

    public async Task<List<Pet>> SearchPetsAsync(string? search = null, int? limit = null, int? offset = null)
    {
        var pets = _dbContext.Pets.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();

            var matchingGenres = Genre.List
                .Where(g => g.Name.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            pets = pets.Where(pet =>
                EF.Functions.Like(pet.Name, $"%{searchTerm}%") ||
                (pet.FavouriteGenre != null && matchingGenres.Contains(pet.FavouriteGenre)));
        }

        var orderedPets = pets
            .OrderBy(p => p.Name)
            .ThenBy(p => p.Id);

        IQueryable<Pet> page = orderedPets;

        if (offset.HasValue)
            page = page.Skip(offset.Value);

        if (limit.HasValue)
            page = page.Take(limit.Value);

        return await page.ToListAsync();
    }

    public Task UpdatePetAsync(Pet pet)
    {
        _dbContext.Pets.Update(pet);

        return Task.CompletedTask;
    }
}
