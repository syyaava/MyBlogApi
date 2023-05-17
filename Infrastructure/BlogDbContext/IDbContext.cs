namespace Infrastructure.BlogDbContext
{
    public interface IDbContext<T>
    {
        public bool Add(T entity);
        public bool Delete(T entity);
        public T GetById(string id);
        public bool Update(T oldEntity,T newEntity);
    }
}
