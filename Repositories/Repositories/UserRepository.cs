namespace Api.Repositories.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(Context context) : base(context) {}

        public async Task<User?> FindAsync(string identification)
        {
            return await (from u in _context.Users
                          where u.Username == identification || u.Email == identification
                          select u)
                          .Include(x => x.UserRoles)
                          .Include(x => x.TokenFunctions)
                          .FirstOrDefaultAsync();
        }

        public async Task<User> InsertAsync(User user, IList<UserRole>? userRoles, TokenFunction tokenFunction)
        {
            using (var dbContextTransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    user.UserRoles = null;
                    await _context.Set<User>().AddAsync(user);
                    await _context.SaveChangesAsync();

                    tokenFunction.UserId = user.Id;
                    await _context.Set<TokenFunction>().AddAsync(tokenFunction);
                    await _context.SaveChangesAsync();

                    if (userRoles != null && userRoles.Count() > 0)
                    {
                        foreach (var item in userRoles)
                            item.UserId = user.Id;

                        await _context.Set<UserRole>().AddRangeAsync(userRoles);
                        await _context.SaveChangesAsync();
                    }

                    await dbContextTransaction.CommitAsync();
                    return user;
                }
                catch (Exception)
                {
                    await dbContextTransaction.RollbackAsync();

                    throw;
                }
            }
        }
    }
}
