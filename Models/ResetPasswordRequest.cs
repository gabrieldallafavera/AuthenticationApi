namespace Api.Models
{
    public class ResetPasswordRequest
    {
        [Required, RegularExpression(@"^(?=.*[A-Z])(?=.*[!#@$%&])(?=.*[0-9])(?=.*[a-z]).{8,}$", ErrorMessage = "Password doesn't follow the standards.")]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password", ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
