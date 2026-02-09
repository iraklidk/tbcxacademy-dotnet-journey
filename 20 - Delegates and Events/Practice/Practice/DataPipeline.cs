public class DataPipeline<T>
{
    private readonly List<Func<T, bool>> filters = new();
    private readonly List<Func<T, object>> transforms = new();

    // add filter
    public DataPipeline<T> AddFilter(Func<T, bool> filter)
    {
        filters.Add(filter);
        return this;
    }

    // add transform
    public DataPipeline<T> AddTransform(Func<T, object> transform)
    {
        transforms.Add(transform);
        return this;
    }

    // process input
    public IEnumerable<object> Process(IEnumerable<T> input)
    {
        IEnumerable<T> filtered = input;

        foreach (var f in filters)
            filtered = filtered.Where(f);

        foreach (var t in transforms)
            foreach (var item in filtered)
                yield return t(item);
    }
}