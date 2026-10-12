namespace BookyPets.Contracts.Sessions;

public record StartSessionRequest(Guid ProgressId, Guid? PetId);
