using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsWebsite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Data.Mapping
{
    public class UserCategoryMapping : IEntityTypeConfiguration<UserCategory>
    {
        public void Configure(EntityTypeBuilder<UserCategory> builder)
        {
            builder.HasKey(t => new { t.CategoryId, t.UserId });
            builder
              .HasOne(p => p.User)
              .WithMany(t => t.UserCategories)
              .HasForeignKey(f => f.UserId);

            builder
               .HasOne(p => p.Category)
               .WithMany(t => t.UserCategories)
               .HasForeignKey(f => f.CategoryId);
        }
    }
}
