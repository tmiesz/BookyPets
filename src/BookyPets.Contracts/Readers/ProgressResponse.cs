namespace BookyPets.Contracts.Readers;

public record ProgressResponse(Guid Id, Guid BookId, int CurrentPage, int TotalPages);
