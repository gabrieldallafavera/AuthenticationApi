using Api.Repositories.Repositories;
using Api.Services.Services;

namespace Api
{
    public static class Scopes
    {
        public static void OnScopeCreating(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenFunctionRepository, TokenFunctionRepository>();
        }
    }
}
