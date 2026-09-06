using DomainGenre = BookyPets.Domain.BookAggregate.Genre;

namespace BookyPets.Api.Common;

public static partial class IconResolver
{
    public static class Genre
    {
        private static readonly Dictionary<DomainGenre, string> _iconMap = new()
        {
            [DomainGenre.Science] = "/icons/books/science.png",
            [DomainGenre.Technology] = "/icons/books/technology.png",
            [DomainGenre.Philosophy] = "/icons/books/philosophy.png",
            [DomainGenre.Psychology] = "/icons/books/psychology.png",
            [DomainGenre.History] = "/icons/books/history.png",
            [DomainGenre.Fiction] = "/icons/books/fiction.png",
            [DomainGenre.Fantasy] = "/icons/books/fantasy.png",
            [DomainGenre.Biography] = "/icons/books/biography.png",
            [DomainGenre.Educational] = "/icons/books/educational.png",
        };

        public static string Resolve(DomainGenre genre) =>
            _iconMap.TryGetValue(genre, out var url)
                ? url
                : "/icons/books/default.png";
    }
}
