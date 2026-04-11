namespace BookyPets.Contracts.Books;

public record BookResponse(Guid Id, string Title, string Author, Genre Genre, int PageCount);
