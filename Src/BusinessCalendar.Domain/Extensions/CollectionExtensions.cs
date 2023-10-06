namespace BusinessCalendar.Domain.Extensions;

public static class CollectionExtensions
{
    /// <summary>
    /// Enables collection initializers with another collection as an input
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="source"></param>
    /// <typeparam name="TCollection"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    /// <returns></returns>
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