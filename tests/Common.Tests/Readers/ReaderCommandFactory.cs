using BookyPets.Application.Authentication.Commands;
using BookyPets.Application.Authentication.Queries;
using BookyPets.Application.Readers.Commands.AcquireBook;
using BookyPets.Application.Readers.Commands.AcquirePet;
using BookyPets.Application.Readers.Queries.GetProgress;
using BookyPets.Domain.Tests.TestConstants;

namespace Common.Tests.Readers;

public static class ReaderCommandFactory
{
    public static RegisterCommand CreateRegisterCommand(
        string? firstName = null,
        string? lastName = null,
        string? email = null,
        string? password = null)
    {
        return new RegisterCommand(
            FirstName: firstName ?? Constants.Reader.FirstName,
            LastName: lastName ?? Constants.Reader.LastName,
            Email: email ?? Constants.Reader.Email,
            Password: password ?? Constants.Reader.Password);
    }

    public static LoginQuery CreateLoginQuery(
        string? email = null,
        string? password = null)
    {
        return new LoginQuery(
            Email: email ?? Constants.Reader.Email,
            Password: password ?? Constants.Reader.Password);
    }

    public static AcquireBookCommand CreateAcquireBookCommand(
        Guid? bookId = null)
    {
        return new AcquireBookCommand(
           BookId: bookId ?? Constants.Book.Id
        );
    }

    public static AcquirePetCommand CreateAcquirePetCommand(
        Guid? petId = null)
    {
        return new AcquirePetCommand(
           PetId: petId ?? Constants.Pet.Id
        );
    }

    public static GetProgressQuery CreateGetProgressQuery(
    Guid? progressId = null)
    {
        return new GetProgressQuery(
            ProgressId: progressId ?? Constants.Progress.Id);

    }
}
