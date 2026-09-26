namespace BookyPets.Application.Sessions.Common;

public record ActiveSessionResult(Guid Id, Guid ProgressId, Guid? PetId, DateTime StartTime, bool IsStale);
