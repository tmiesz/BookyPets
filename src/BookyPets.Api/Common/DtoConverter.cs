using BookyPets.Contracts.Books;
using BookyPets.Contracts.Readers;
using BookyPets.Contracts.Sessions;
using BookyPets.Contracts.Pets;
using DomainGenre = BookyPets.Domain.BookAggregate.Genre;
using DomainAccountType = BookyPets.Domain.ReaderAggregate.AccountType;
using DomainSessionStatus = BookyPets.Domain.SessionAggregate.SessionStatus;
using DomainSpecies = BookyPets.Domain.PetAggregate.Species;

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

    public static Species ToDto(DomainSpecies species) => MapToEnum<Species>(species.Name);
    public static bool TryToDomain(Species species, out DomainSpecies domainSpecies)
         => DomainSpecies.TryFromName(species.ToString(), out domainSpecies!);

    private static T MapToEnum<T>(string name) where T : struct, Enum
    {
        if (Enum.TryParse<T>(name, true, out var result))
        {
            return result;
        }

        throw new InvalidOperationException($"{name} does not exist");
    }
}
