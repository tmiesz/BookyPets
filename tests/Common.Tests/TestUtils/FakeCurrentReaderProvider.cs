using BookyPets.Application.Common.Interfaces;
using BookyPets.Application.Common.Models;
using BookyPets.Domain.Tests.TestConstants;

namespace Common.Tests.TestUtils;

public class FakeCurrentReaderProvider : ICurrentReaderProvider
{
    private CurrentReader _currentReader = new(
        Id: Constants.Reader.Id,
        Permissions: Constants.Reader.Permissions,
        Roles: Constants.Reader.Roles);

    public void SetCurrentReader(Guid id) =>
        _currentReader = _currentReader with { Id = id };

    public CurrentReader GetCurrentReader() => _currentReader;
}
