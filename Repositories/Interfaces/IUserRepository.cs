namespace Api.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> FindAsync(string identification);
        Task<User> InsertAsync(User user, IList<UserRole>? userRoles, TokenFunction tokenFunction);
    }
}
