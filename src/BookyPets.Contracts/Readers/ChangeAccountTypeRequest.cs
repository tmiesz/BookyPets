using System.Text.Json.Serialization;

namespace BookyPets.Contracts.Readers;

public record ChangeAccountTypeRequest([property: JsonRequired] AccountType AccountType);
