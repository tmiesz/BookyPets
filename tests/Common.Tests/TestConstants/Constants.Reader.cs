using BookyPets.Domain.ReaderAggregate;

namespace BookyPets.Domain.Tests.TestConstants;

public static partial class Constants
{

    public static class Reader
    {
        public static readonly Guid Id = Guid.NewGuid();
        public const string FirstName = "Bob";
        public const string LastName = "Smith";
        public const string Email = "bob@smith.com";
        public const string Password = "Admin123!@#A";
        public const string PasswordHash = "hash";

        public static readonly IReadOnlyList<string> Permissions =
        [
            "books:acquire",
            "pets:acquire",
            "sessions:start",
            "sessions:finish"
        ];

        public static readonly IReadOnlyList<string> Roles = [Role.Admin];
    }
}
