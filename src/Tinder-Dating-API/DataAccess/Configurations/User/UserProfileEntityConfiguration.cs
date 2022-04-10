using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tinder_Dating_API.Entites;

namespace Tinder_Dating_API.DataAccess.Configurations.User
{
    public class UserProfileEntityConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasOne(p => p.User)
                   .WithOne(u => u.Profile);

            builder.HasMany(p => p.Images)
                   .WithOne(i => i.Profile)
                   .HasForeignKey(c => c.UserProfileId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Address)
                   .WithOne(a => a.Profile)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
