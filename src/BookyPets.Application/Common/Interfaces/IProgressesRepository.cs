using BookyPets.Domain.BookAggregate;

namespace BookyPets.Application.Common.Interfaces;

public interface IProgressesRepository
{
    Task AddProgressAsync(Progress progress);
    Task<Progress?> GetProgressAsync(Guid progressId);
    Task<List<Progress>> GetProgressesAsync(List<Guid> progressIds);
    Task UpdateProgressAsync(Progress progress);
}
