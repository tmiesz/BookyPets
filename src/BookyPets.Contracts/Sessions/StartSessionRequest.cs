namespace BookyPets.Contracts.Sessions;

public record StartSessionRequest(Guid ReaderId, Guid ProgressId,  Guid? PetId);
