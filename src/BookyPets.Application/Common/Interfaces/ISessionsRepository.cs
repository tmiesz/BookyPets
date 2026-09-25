using BookyPets.Application.Common.Models;
using BookyPets.Domain.SessionAggregate;

namespace BookyPets.Application.Common.Interfaces;

public interface ISessionsRepository
{
    Task AddSessionAsync(Session session);
    Task<Session?> GetSessionAsync(Guid sessionId);
    Task UpdateSessionAsync(Session session);
    Task<Session?> GetActiveSessionAsync(Guid readerId);
    Task<ActiveSessionInfo?> GetActiveSessionInfoAsync(Guid readerId);
}
