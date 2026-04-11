using BookyPets.Domain.ReaderAggregate;

namespace BookyPets.Application.Common.Interfaces;

public interface IReadersRepository
{
    Task AddReaderAsync(Reader reader);
    Task<bool> ExistsByEmailAsync(string email);
    Task<Reader?> GetByEmailAsync(string email);
    Task<Reader?> GetByIdAsync(Guid readerId);
    Task UpdateReaderAsync(Reader reader);
}
