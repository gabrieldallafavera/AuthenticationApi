namespace Api.Database.Entities
{
    [Table("TokenFunction")]
    public class TokenFunction : BaseEntity
    {
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public int Type { get; set; }
        public DateTime ExpiresAt { get; set; }

        public User? User { get; set; }
    }
}
