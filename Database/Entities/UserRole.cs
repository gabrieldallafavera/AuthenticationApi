namespace Api.Database.Entities
{
    [Table("UserRole")]
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public string Role { get; set; } = string.Empty;

        public User? User { get; set; }
    }
}
