using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BookyPets.Infrastructure.Common.Persistence;

public class ListOfStringsConverter : ValueConverter<List<string>, string>
{
    public ListOfStringsConverter()
        : base(
            v => string.Join(',', v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
    {
    }
}

public class ListOfStringsComparer : ValueComparer<List<string>>
{
    public ListOfStringsComparer() : base(
        (l1, l2) => l1!.SequenceEqual(l2!),
        l => l.Aggregate(0, (hash, s) => HashCode.Combine(hash, s.GetHashCode())),
        l => l.ToList())
    {
    }
}
