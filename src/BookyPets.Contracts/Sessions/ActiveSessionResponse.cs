namespace BookyPets.Contracts.Sessions;

public record ActiveSessionResponse(Guid Id, Guid ProgressId, Guid? PetId, DateTime StartTime, bool IsStale);
