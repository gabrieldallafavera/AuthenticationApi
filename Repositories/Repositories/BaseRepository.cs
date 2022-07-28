namespace Api.Repositories.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        public readonly Context _context;

        public BaseRepository(Context context)
        {
            _context = context;
        }

        public async Task<TEntity?> FindAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IList<TEntity>?> ListAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<object> PaginateAsync(int page = 1, int itemsPerPage = 20)
        {
            return PaginationBuilder<TEntity>.ToPagination(await _context.Set<TEntity>().ToListAsync(), page, itemsPerPage);
        }

        public async Task<TEntity> InsertAsync(TEntity data)
        {
            await _context.Set<TEntity>().AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<IList<TEntity>> InsertRangeAsync(IList<TEntity> data)
        {
            await _context.Set<TEntity>().AddRangeAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<TEntity> UpdateAsync(TEntity data)
        {
            _context.Set<TEntity>().Update(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            if (entity == null)
                throw new NotFoundException("Not found.");
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
