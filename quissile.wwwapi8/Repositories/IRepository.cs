namespace quissile.wwwapi8.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> Insert(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<T> Update (T entity);
        Task<T> DeleteById(int id);
        Task<T> GetById(int id);
    }
}
