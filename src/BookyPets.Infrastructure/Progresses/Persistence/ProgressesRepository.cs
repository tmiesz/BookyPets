using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
using BookyPets.Infrastructure.Common.Persistence;

namespace BookyPets.Infrastructure.Progresses.Persistence;

public class ProgressesRepository : IProgressesRepository
{
    private readonly BookyPetsDbContext _dbContext;

    public ProgressesRepository(BookyPetsDbContext dbcontext)
    {
        _dbContext = dbcontext;
    }

    public async Task AddProgressAsync(Progress progress)
    {
        await _dbContext.Progresses.AddAsync(progress);
    }

    public async Task<Progress?> GetProgressAsync(Guid progressId)
    {
        return await _dbContext.Progresses.FindAsync(progressId);
    }

    public Task UpdateProgressAsync(Progress progress)
    {
        _dbContext.Progresses.Update(progress);

        return Task.CompletedTask;
    }
}
