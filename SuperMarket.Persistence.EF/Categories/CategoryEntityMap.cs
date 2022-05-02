using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CategoryEntityMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> _)
    {
        _.ToTable("Categories");
        _.HasKey(p => p.Id);
        _.Property(p => p.Id).ValueGeneratedOnAdd();
        _.Property(p => p.Name).HasMaxLength(50).IsRequired();
        _.HasMany(p => p.Products).WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.ClientNoAction);
    }
}