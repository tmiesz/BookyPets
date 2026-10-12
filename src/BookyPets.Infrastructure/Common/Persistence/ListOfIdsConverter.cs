using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BookyPets.Infrastructure.Common.Persistence;

public class ListOfIdsConverter(ConverterMappingHints? mappingHints = null) : ValueConverter<List<Guid>, string>(
        v => string.Join(',', v),
        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList(),
        mappingHints)
{
}

public class ListOfIdsComparer : ValueComparer<List<Guid>>
{
    public ListOfIdsComparer() : base(
      (t1, t2) => t1!.SequenceEqual(t2!),
      t => t.Aggregate(0, (hash, id) => HashCode.Combine(hash, id)),
      t => t.ToList())
    {
    }
}
