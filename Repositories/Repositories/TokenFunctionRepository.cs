namespace Api.Repositories.Repositories
{
    public class TokenFunctionRepository : BaseRepository<TokenFunction>, ITokenFunctionRepository
    {
        public TokenFunctionRepository(Context context) : base(context) {}

        public async Task<TokenFunction?> FindAsync(string token)
        {
            return await (from tf in _context.TokenFunctions
                          where tf.Token == token
                          select tf)
                          .Include(x => x.User)
                          .ThenInclude(x => x!.UserRoles)
                          .FirstOrDefaultAsync();
        }
    }
}
