using BookyPets.Contracts.Books;
using BookyPets.Contracts.Readers;
using BookyPets.Contracts.Sessions;
using DomainGenre = BookyPets.Domain.BookAggregate.Genre;
using DomainAccountType = BookyPets.Domain.ReaderAggregate.AccountType;
using DomainSessionStatus = BookyPets.Domain.SessionAggregate.SessionStatus;

namespace BookyPets.Api.Common;

public static class DtoConverter
{
    public static Genre ToDto(DomainGenre genre) => MapToEnum<Genre>(genre.Name);
    public static bool TryToDomain(Genre genre, out DomainGenre domainGenre)
        => DomainGenre.TryFromName(genre.ToString(), out domainGenre!);

    public static AccountType ToDto(DomainAccountType accountType) => MapToEnum<AccountType>(accountType.Name);
    public static bool TryToDomain(AccountType accountType, out DomainAccountType domainAccountType)
         => DomainAccountType.TryFromName(accountType.ToString(), out domainAccountType!);

    public static SessionStatus ToDto(DomainSessionStatus sessionStatus) => MapToEnum<SessionStatus>(sessionStatus.Name);
    public static bool TryToDomain(SessionStatus sessionStatus, out DomainSessionStatus domainSessionStatus)
         => DomainSessionStatus.TryFromName(sessionStatus.ToString(), out domainSessionStatus!);

    private static T MapToEnum<T>(string name) where T : struct, Enum
    {
        if (Enum.TryParse<T>(name, true, out var result))
        {
            return result;
        }

        throw new InvalidOperationException($"{name} does not exist");
    }
}
