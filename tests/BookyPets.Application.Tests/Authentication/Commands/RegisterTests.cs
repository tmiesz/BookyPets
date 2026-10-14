using BookyPets.Application.Tests.Common;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using Common.Tests.Readers;
using Common.Tests.TestConstants;

namespace BookyPets.Application.Tests.Authentication.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class RegisterTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();

    [Fact]
    public async Task Register_WhenValidCommand_ShouldRegisterReaderAndIssueToken()
    {
        var result = await _mediator.SendAsync(ReaderCommandFactory.CreateRegisterCommand());

        Assert.True(result.IsSuccess);
        Assert.False(string.IsNullOrWhiteSpace(result.Value.Token));
        Assert.Equal(Constants.Reader.Email, result.Value.Reader.Email);
    }

    [Fact]
    public async Task Register_WhenEmailHasMixedCase_ShouldStoreTheNormalizedEmail()
    {
        var command = ReaderCommandFactory.CreateRegisterCommand(email: "Bob@Smith.com");

        var result = await _mediator.SendAsync(command);

        Assert.True(result.IsSuccess);
        Assert.Equal("bob@smith.com", result.Value.Reader.Email);
    }

    [Fact]
    public async Task Register_WhenEmailAlreadyExists_ShouldReturnConflict()
    {
        await _mediator.SendAsync(ReaderCommandFactory.CreateRegisterCommand());

        var result = await _mediator.SendAsync(ReaderCommandFactory.CreateRegisterCommand());

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
    }

    [Fact]
    public async Task Register_WhenEmailDiffersOnlyByCase_ShouldReturnConflict()
    {
        await _mediator.SendAsync(ReaderCommandFactory.CreateRegisterCommand(email: "bob@smith.com"));

        var result = await _mediator.SendAsync(ReaderCommandFactory.CreateRegisterCommand(email: "BOB@smith.com"));

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
    }

    [Theory]
    [InlineData("", "Smith", "bob@smith.com", "Admin123!@#A", "FirstName")]
    [InlineData("Bob", "", "bob@smith.com", "Admin123!@#A", "LastName")]
    [InlineData("Bob", "Smith", "not-an-email", "Admin123!@#A", "Email")]
    [InlineData("Bob", "Smith", "", "Admin123!@#A", "Email")]
    [InlineData("Bob", "Smith", "bob@smith.com", "", "Password")]
    public async Task Register_WhenCommandContainsInvalidData_ShouldReturnValidationError(
        string firstName,
        string lastName,
        string email,
        string password,
        string expectedCode)
    {
        var command = ReaderCommandFactory.CreateRegisterCommand(firstName, lastName, email, password);

        var result = await _mediator.SendAsync(command);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        Assert.Equal(expectedCode, result.Error.Code);
    }
}
