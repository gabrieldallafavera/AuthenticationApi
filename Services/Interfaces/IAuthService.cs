namespace Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponse> Register(UserRequest userRequest);
        Task<TokenResponse> Login(LoginRequest loginRequest);
        Task<TokenResponse> RefreshToken(string token);
        Task ResendVerifyEmail(string identification);
        Task VerifyEmail(string token);
        Task ForgotPassword(string identification);
        Task ResetPassword(string token, ResetPasswordRequest resetPasswordRequest);
    }
}
