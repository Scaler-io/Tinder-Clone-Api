﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tinder_Dating_API.Entites;

namespace Tinder_Dating_API.DataAccess.Configurations.User
{
    public class AppUserEntityConfiguration: IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {

            builder.HasOne(u => u.Profile)
                    .WithOne(p => p.User)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(u => u.UserName).IsUnique();    
            
            builder.HasMany(ur => ur.UserRoles)
                    .WithOne(u => u.User)
                    .HasForeignKey(u => u.UserId)
                    .IsRequired();
        }
    }
}
