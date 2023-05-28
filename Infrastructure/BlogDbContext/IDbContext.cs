namespace Infrastructure.BlogDbContext
{
    public interface IDbContext<T>
    {
        IEnumerable<T> Values { get; }
    }
}
