using BookyPets.Domain.ReaderAggregate;

namespace BookyPets.Application.Authentication.Common;

public record AuthenticationResult(Reader Reader, string Token);
