using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookyPets.Infrastructure.Sessions;

public class SessionsRepository(BookyPetsDbContext dbcontext) : ISessionsRepository
{
    private readonly BookyPetsDbContext _dbContext = dbcontext;

    public async Task AddSessionAsync(Session session)
    {
        await _dbContext.Sessions.AddAsync(session);
    }

    public async Task<Session?> GetActiveSession(Guid readerId)
    {
        return await _dbContext.Sessions.FirstOrDefaultAsync(s =>
                EF.Property<Guid>(s, "_readerId") == readerId &&
                    s.Status == SessionStatus.Active);
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
