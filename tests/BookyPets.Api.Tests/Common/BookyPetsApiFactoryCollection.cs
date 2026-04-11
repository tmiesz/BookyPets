namespace BookyPets.Api.Tests.Common;

[CollectionDefinition(CollectionName)]
public class BookyPetsApiFactoryCollection : ICollectionFixture<BookyPetsApiFactory>
{
    public const string CollectionName = "BookyPetsApiFactoryCollection";
}
