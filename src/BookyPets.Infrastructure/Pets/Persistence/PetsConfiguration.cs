using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.PetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookyPets.Infrastructure.Pets.Persistence;

public class PetsConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.Name).IsRequired();

        builder.Property(p => p.FavouriteGenre)
            .HasConversion(
                genre => genre != null ? genre.Name : null,
                name => name != null ? Genre.FromName(name) : null);

        builder.Property(p => p.Species)
            .HasConversion(
                species => species.Name,
                name => Species.FromName(name)!);

        builder.Property("_experience").HasColumnName("Experience");

        builder.Ignore("_domainEvents");
    }
}
