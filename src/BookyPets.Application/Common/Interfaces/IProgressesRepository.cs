using BookyPets.Domain.BookAggregate;

namespace BookyPets.Application.Common.Interfaces;

public interface IProgressesRepository
{
    Task AddProgressAsync(Progress progress);
    Task<Progress?> GetProgressAsync(Guid progressId);
    Task UpdateProgressAsync(Progress progress);
}
