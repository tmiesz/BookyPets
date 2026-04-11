using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.SessionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookyPets.Infrastructure.Sessions;

public class SessionsConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property<Guid>("_readerId")
            .HasColumnName("ReaderId")
            .IsRequired();

        builder.Property<Guid>("_progressId")
            .HasColumnName("ProgressId")
            .IsRequired();

        builder.Property<Guid>("_bookId")
            .HasColumnName("BookId")
            .IsRequired();

        builder.Property<Genre>("_genre")
            .HasColumnName("Genre")
            .HasConversion(
                genre => genre.Name,
                name => Genre.FromName(name)!);

        builder.Property<Guid?>("_petId")
            .HasColumnName("PetId");

        builder.Property<DateTime>("_startTime")
            .HasColumnName("StartTime")
            .IsRequired();

        builder.Property(s => s.Status)
            .HasConversion(
                status => status.Name,
                name => SessionStatus.FromName(name)!);

        builder.Property(s => s.PagesRead)
            .IsRequired();

        builder.Property(s => s.EndTime);

        builder.Ignore("_domainEvents");
    }
}
