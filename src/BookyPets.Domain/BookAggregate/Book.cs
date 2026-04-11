using BookyPets.Domain.Common;

namespace BookyPets.Domain.BookAggregate;

public class Book : Entity
{
    public string Title { get; init; }
    public string Author { get; init; }
    public Genre Genre { get; init; }
    public int PageCount { get; init; }

    private Book()
    {
        Title = null!;
        Author = null!;
        Genre = null!;
    }

    public Book(string title, string author, Genre genre, int pageCount, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        Title = title;
        Author = author;
        Genre = genre;
        PageCount = pageCount;
    }
}
