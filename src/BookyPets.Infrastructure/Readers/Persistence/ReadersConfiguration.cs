using BookyPets.Domain.ReaderAggregate;
using BookyPets.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookyPets.Infrastructure.Readers.Persistence;

public class ReadersConfiguration : IEntityTypeConfiguration<Reader>
{
    public void Configure(EntityTypeBuilder<Reader> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.Property(r => r.FirstName).IsRequired();
        builder.Property(r => r.LastName).IsRequired();
        builder.Property(r => r.Email).IsRequired();

        builder.Property<string>("_passwordHash")
            .HasColumnName("PasswordHash")
            .IsRequired();

        builder.Property(r => r.AccountType)
            .HasConversion(
                accountType => accountType.Name,
                name => AccountType.FromName(name)!);

        builder.Property<List<string>>("_roles")
            .HasColumnName("Roles")
            .HasListOfStringsConverter();

        builder.Property<List<Guid>>("_petIds")
            .HasColumnName("PetIds")
            .HasListOfIdsConverter();

        builder.Property<List<Guid>>("_bookIds")
            .HasColumnName("BookIds")
            .HasListOfIdsConverter();

        builder.Property<List<Guid>>("_progressIds")
            .HasColumnName("ProgressIds")
            .HasListOfIdsConverter();

        builder.Ignore("_domainEvents");
    }
}
