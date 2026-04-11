namespace BookyPets.Contracts.Sessions;

public record CompleteSessionRequest(Guid SessionId, int PagesRead);
