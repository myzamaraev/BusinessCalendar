namespace BusinessCalendar.Domain.Extensions;

public static class CollectionExtensions
{
    public static TCollection Add<TCollection, TItem>(
        this TCollection destination,
        IEnumerable<TItem> source)
        where TCollection : ICollection<TItem>
    {
        ArgumentNullException.ThrowIfNull(destination);
        ArgumentNullException.ThrowIfNull(source);

        if (destination is List<TItem> list)
        {
            list.AddRange(source);
            return destination;
        }

        foreach (var item in source)
        {
            destination.Add(item);
        }

        return destination;
    }
}