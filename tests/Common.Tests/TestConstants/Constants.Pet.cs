using ContractsGenre = BookyPets.Contracts.Books.Genre;
using DomainGenre = BookyPets.Domain.BookAggregate.Genre;
using ContractsSpecies = BookyPets.Contracts.Pets.Species;
using DomainSpecies = BookyPets.Domain.PetAggregate.Species;

namespace BookyPets.Domain.Tests.TestConstants;

public static partial class Constants
{
    public static class Pet
    {
        public static readonly Guid Id = Guid.NewGuid();
        public static readonly string Name = "David";
        public static readonly DomainGenre FavouriteGenre = DomainGenre.Educational;
        public static readonly ContractsGenre FavouriteContractsGenre = ContractsGenre.Educational;
        public static readonly DomainSpecies Species = DomainSpecies.Bear;
        public static readonly ContractsSpecies ContractSpecies = ContractsSpecies.Bear;
    }
}
