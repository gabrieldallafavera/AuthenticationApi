namespace Api.Database.Entities
{
    [Table("User")]
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[8];
        public byte[] PasswordSalt { get; set; } = new byte[8];
        public DateTime? VerifiedAt { get; set; }

        public IList<UserRole>? UserRoles { get; set; }
        public IList<TokenFunction>? TokenFunctions { get; set; }
    }
}
