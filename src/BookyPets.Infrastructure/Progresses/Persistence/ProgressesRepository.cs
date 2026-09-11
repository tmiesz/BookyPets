using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
using BookyPets.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookyPets.Infrastructure.Progresses.Persistence;

public class ProgressesRepository(BookyPetsDbContext dbcontext) : IProgressesRepository
{
    private readonly BookyPetsDbContext _dbContext = dbcontext;

    public async Task AddProgressAsync(Progress progress)
    {
        await _dbContext.Progresses.AddAsync(progress);
    }

    public async Task<Progress?> GetProgressAsync(Guid progressId)
    {
        return await _dbContext.Progresses.FindAsync(progressId);
    }

    public async Task<List<Progress>> GetProgressesAsync(List<Guid> progressIds)
    {
        return await _dbContext.Progresses
            .Where(p => progressIds.Contains(p.Id))
            .ToListAsync();
    }

    public Task UpdateProgressAsync(Progress progress)
    {
        _dbContext.Progresses.Update(progress);

        return Task.CompletedTask;
    }
}
