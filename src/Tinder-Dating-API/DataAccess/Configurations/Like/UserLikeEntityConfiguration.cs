using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tinder_Dating_API.Entites;

namespace Tinder_Dating_API.DataAccess.Configurations.Like
{
    public class UserLikeEntityConfiguration : IEntityTypeConfiguration<UserLike>
    {
        public void Configure(EntityTypeBuilder<UserLike> builder)
        {
            builder.HasKey(x => new { x.SourceUserId, x.LikedUserId});

            builder.HasOne(x => x.SourceUser)
                   .WithMany(x => x.LikedUsers)
                   .HasForeignKey(x => x.SourceUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.LikedUser)
                   .WithMany(x => x.LikedByUser)
                   .HasForeignKey(x => x.LikedUserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }          
    }
}
