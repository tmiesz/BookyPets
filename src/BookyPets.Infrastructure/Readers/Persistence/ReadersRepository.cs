using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookyPets.Infrastructure.Readers.Persistence;

public class ReadersRepository(BookyPetsDbContext _dbContext) : IReadersRepository
{
    public async Task AddReaderAsync(Reader reader)
    {
        await _dbContext.Readers.AddAsync(reader);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbContext.Readers.AnyAsync(reader => reader.Email == email);
    }

    public async Task<Reader?> GetByEmailAsync(string email)
    {
        return await _dbContext.Readers.FirstOrDefaultAsync(reader => reader.Email == email);
    }

    public async Task<Reader?> GetByIdAsync(Guid readerId)
    {
        return await _dbContext.Readers.FindAsync(readerId);
    }

    public Task UpdateReaderAsync(Reader reader)
    {
        _dbContext.Readers.Update(reader);

        return Task.CompletedTask;
    }
}
