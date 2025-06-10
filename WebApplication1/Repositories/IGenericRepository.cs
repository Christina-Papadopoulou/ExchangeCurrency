namespace WalletAppication.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T> GetByIdAsync(long id);
        Task<List<T>> GetAllAsync();
        Task UpdateAsync(T entity);
    }
}
