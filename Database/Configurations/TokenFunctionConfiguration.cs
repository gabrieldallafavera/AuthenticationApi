using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Database.Configurations
{
    public class TokenFunctionConfiguration : BaseEntityConfiguration<TokenFunction>
    {
        public override void Configure(EntityTypeBuilder<TokenFunction> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(x => x.User)
                .WithMany(c => c.TokenFunctions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasIndex(x => x.Token)
                .IsUnique();
            builder
                .Property(x => x.Token)
                .HasColumnType("varchar(200)")
                .IsRequired();

            builder
                .Property(x => x.ExpiresAt)
                .HasColumnType("datetime")
                .IsRequired();
        }
    }
}
