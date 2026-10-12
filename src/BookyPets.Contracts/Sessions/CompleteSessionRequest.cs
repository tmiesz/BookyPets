using System.Text.Json.Serialization;

namespace BookyPets.Contracts.Sessions;

public record CompleteSessionRequest(Guid SessionId, [property: JsonRequired] int PagesRead);
