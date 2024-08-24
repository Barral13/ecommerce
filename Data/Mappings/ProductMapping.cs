using ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ecommerce.Data.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);

            builder.Property(x => x.Image)
                .IsRequired()
                .HasColumnType("VARCHAR")
                .HasMaxLength(255);

            builder.Property(x => x.Description)
                .IsRequired(false)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255);

            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnType("DECIMAL(18,2)");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder
                .HasIndex(x => x.Slug, "IX_Product_Slug")
                .IsUnique();

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
