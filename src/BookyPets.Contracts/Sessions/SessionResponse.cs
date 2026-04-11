namespace BookyPets.Contracts.Sessions;

public record SessionResponse(Guid Id, SessionStatus Status, int PagesRead, DateTime? EndTime);
