using BookyPets.Domain.ReaderAggregate;
using BookyPets.Domain.Tests.TestConstants;

namespace Common.Tests.Readers;

public static class ReaderFactory
{
    public static Reader CreateReader(
        string? firstName = null,
        string? lastName = null,
        string? email = null,
        string? passwordHash = null,
        Guid? id = null)
    {
        return new Reader(
            firstName: firstName ?? Constants.Reader.FirstName,
            lastName: lastName ?? Constants.Reader.LastName,
            email: email ?? Constants.Reader.Email,
            passwordHash: passwordHash ?? Constants.Reader.PasswordHash,
            id: id ?? Constants.Reader.Id
        );
    }

    public static Reader CreateProReader(
        string? firstName = null,
        string? lastName = null,
        string? email = null,
        string? passwordHash = null,
        Guid? id = null)
    {
        var reader = CreateReader(firstName, lastName, email, passwordHash, id);
        reader.ChangeAccountType(AccountType.Pro);

        return reader;
    }
}
