namespace Api.Repositories.Interfaces
{
    public interface ITokenFunctionRepository : IBaseRepository<TokenFunction>
    {
        Task<TokenFunction?> FindAsync(string token);
    }
}
