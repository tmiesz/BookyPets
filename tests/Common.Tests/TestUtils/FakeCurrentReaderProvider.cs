using BookyPets.Application.Common.Interfaces;
using BookyPets.Application.Common.Models;
using Common.Tests.TestConstants;

namespace Common.Tests.TestUtils;

public class FakeCurrentReaderProvider : ICurrentReaderProvider
{
    private CurrentReader _currentReader = CreateDefault();

    private static CurrentReader CreateDefault() => new(
        Id: Constants.Reader.Id,
        Permissions: Constants.Reader.Permissions,
        Roles: Constants.Reader.Roles);

    public void SetCurrentReader(Guid id) =>
        _currentReader = _currentReader with { Id = id };

    public void SetCurrentReader(Guid id, IReadOnlyList<string> permissions, IReadOnlyList<string> roles) =>
        _currentReader = new CurrentReader(id, permissions, roles);

    public void Reset() => _currentReader = CreateDefault();

    public CurrentReader GetCurrentReader() => _currentReader;
}
