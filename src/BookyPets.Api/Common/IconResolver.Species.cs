using DomainSpecies = BookyPets.Domain.PetAggregate.Species;

namespace BookyPets.Api.Common;

public static partial class IconResolver
{
    public static class Species
    {
        private static readonly Dictionary<DomainSpecies, string> _iconMap = new()
        {
            [DomainSpecies.Bear] = "/icons/pets/bear.png",
            [DomainSpecies.Bee] = "/icons/pets/bee.png",
            [DomainSpecies.Bird] = "/icons/pets/bird.png",
            [DomainSpecies.Butterfly] = "/icons/pets/butterfly.png",
            [DomainSpecies.Dog] = "/icons/pets/dog.png",
            [DomainSpecies.Lion] = "/icons/pets/lion.png",
            [DomainSpecies.Tiger] = "/icons/pets/tiger.png",
            [DomainSpecies.Whale] = "/icons/pets/whale.png",
        };

        public static string Resolve(DomainSpecies species) =>
            _iconMap.TryGetValue(species, out var url)
                ? url
                : "/icons/pets/bear.png";
    }
}
