namespace Api.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> FindAsync(int id);
        Task<IList<TEntity>?> ListAsync();
        Task<object> PaginateAsync(int page = 1, int itemsPerPage = 20);
        Task<TEntity> InsertAsync(TEntity data);
        Task<IList<TEntity>> InsertRangeAsync(IList<TEntity> data);
        Task<TEntity> UpdateAsync(TEntity data);
        Task DeleteAsync(int id);
    }
}
