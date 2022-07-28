using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Database.Configurations
{
    public abstract class BaseEntityConfiguration<TBaseEntity> : IEntityTypeConfiguration<TBaseEntity> where TBaseEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TBaseEntity> builder)
        {
            builder
                .Property(x => x.Created)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()")
                .IsRequired();

            builder
                .Property(x => x.Updated)
                .HasColumnType("datetime");

            builder
                .Property(x => x.Deleted)
                .HasColumnType("datetime");
        }
    }
}
