using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Infrastructure.Common.Persistence;

namespace BookyPets.Infrastructure.Sessions;

public class SessionsRepository : ISessionsRepository
{
    private readonly BookyPetsDbContext _dbContext;

    public SessionsRepository(BookyPetsDbContext dbcontext)
    {
        _dbContext = dbcontext;
    }

    public async Task AddSessionAsync(Session session)
    {
        await _dbContext.Sessions.AddAsync(session);
    }

    public async Task<Session?> GetSessionAsync(Guid sessionId)
    {
        return await _dbContext.Sessions.FindAsync(sessionId);
    }

    public Task UpdateSessionAsync(Session session)
    {
        _dbContext.Sessions.Update(session);

        return Task.CompletedTask;
    }
}
