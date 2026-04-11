using BookyPets.Application.Books.Commands;
using BookyPets.Application.Common.Behaviours;
using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using BookyPets.Shared.Validator;
using Common.Tests.Books;
using NSubstitute;

namespace BookyPets.Application.Tests.Behaviours;

public class ValidationBehaviourTests
{
    private readonly ValidationBehaviour<CreateBookCommand, Result<Book>> _validationBehaviour;
    private readonly IValidator<CreateBookCommand> _mockValidator;
    private readonly HandlerDelegate<Result<Book>> _mockNextBehaviour;

    public ValidationBehaviourTests()
    {
        _mockNextBehaviour = Substitute.For<HandlerDelegate<Result<Book>>>();
        _mockValidator = Substitute.For<IValidator<CreateBookCommand>>();
        _validationBehaviour = new ValidationBehaviour<CreateBookCommand, Result<Book>>(_mockValidator);
    }

    [Fact]
    public async Task InvokeBehaviour_WhenValidatorResultIsValid_ShouldInvokeNextBehaviour()
    {
        var createBookRequest = BookCommandFactory.CreateCreateBookCommand();
        var book = BookFactory.CreateBook();

        _mockValidator
            .Validate(createBookRequest)
            .Returns(new ValidationResult());

        _mockNextBehaviour.Invoke().Returns(book);

        var result = await _validationBehaviour.HandleAsync(createBookRequest, _mockNextBehaviour, default);

        Assert.True(result.IsSuccess);
        Assert.Same(result.Value, book);
    }

    [Fact]
    public async Task InvokeBehaviour_WhenValidatorResultIsNotValid_ShouldReturnValidationError()
    {
        var createBookRequest = BookCommandFactory.CreateCreateBookCommand();
        List<ValidationFailure> validationFailures = [new(propertyName: "bad", errorMessage: "bad")];

        _mockValidator
            .Validate(createBookRequest)
            .Returns(new ValidationResult(validationFailures));

        var result = await _validationBehaviour.HandleAsync(createBookRequest, _mockNextBehaviour, default);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        await _mockNextBehaviour.DidNotReceive().Invoke();
    }

    [Fact]
    public async Task InvokeBehaviour_WhenNoValidatorProvided_ShouldInvokeNextBehaviour()
    {
        var createBookRequest = BookCommandFactory.CreateCreateBookCommand();
        var book = BookFactory.CreateBook();
        var behaviourWithoutValidator = new ValidationBehaviour<CreateBookCommand, Result<Book>>();

        _mockNextBehaviour.Invoke().Returns(book);

        var result = await behaviourWithoutValidator.HandleAsync(createBookRequest, _mockNextBehaviour, default);

        Assert.True(result.IsSuccess);
        Assert.Same(result.Value, book);
    }
}
