namespace BookyPets.Application.Common.Models;

public record ActiveSessionInfo(Guid Id, Guid ProgressId, Guid? PetId, DateTime StartTime, DateTime LastHeartbeatAt);
