using BookyPets.Domain.BookAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookyPets.Infrastructure.Progresses.Persistence;

public class ProgressesConfiguration : IEntityTypeConfiguration<Progress>
{
    public void Configure(EntityTypeBuilder<Progress> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property<Guid>("_readerId").HasColumnName("ReaderId").IsRequired();
        builder.Property(p => p.BookId).IsRequired();

        builder.Property(p => p.TotalPages).IsRequired();
        builder.Property(p => p.CurrentPage).IsRequired();
        builder.Property(p => p.Status)
            .HasConversion(status => status.Name, name => BookStatus.FromName(name)!);

        builder.Ignore("_domainEvents");
    }
}
