namespace BookyPets.Application.Common.Models;

public record CurrentReader(Guid Id, IReadOnlyList<string> Permissions, IReadOnlyList<string> Roles);
