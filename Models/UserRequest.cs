namespace Api.Models
{
    public class UserRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, RegularExpression(@"^(?=.*[A-Z])(?=.*[!#@$%&])(?=.*[0-9])(?=.*[a-z]).{8,}$", ErrorMessage = "Password doesn't follow the standards.")]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password", ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public IList<UserRoles>? UserRoles { get; set; }
    }

    public class UserRoles
    {
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
