namespace Api.Database.Entities
{
    public abstract class BaseEntity
    {
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
