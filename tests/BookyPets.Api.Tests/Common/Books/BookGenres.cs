using BookyPets.Contracts.Books;

namespace BookyPets.Api.Tests.Common.Books;

[Collection(BookyPetsApiFactoryCollection.CollectionName)]
public class BookGenres
{
    public static TheoryData<Genre> ListGenres()
    {
        var genres = Enum.GetValues<Genre>().ToList();

        var theoryData = new TheoryData<Genre>();

        foreach (var genre in genres)
        {
            theoryData.Add(genre);
        }

        return theoryData;
    }
}
