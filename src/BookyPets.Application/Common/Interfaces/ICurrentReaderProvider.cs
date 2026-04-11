using BookyPets.Application.Common.Models;

namespace BookyPets.Application.Common.Interfaces;

public interface ICurrentReaderProvider
{
    CurrentReader GetCurrentReader();
}
