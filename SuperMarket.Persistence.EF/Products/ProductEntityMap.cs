using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductEntityMap : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> _)
    {
        _.ToTable("Products");
        _.HasKey(p => p.Id);
        _.Property(p => p.Id).ValueGeneratedOnAdd();
        _.Property(p => p.Name).HasMaxLength(70).IsRequired();
        _.Property(p => p.ProductKey).HasMaxLength(10).IsRequired();
        _.Property(p => p.Price).IsRequired();
        _.Property(p => p.Brand).HasMaxLength(50).IsRequired();
        _.Property(p => p.MinimumAllowableStock).HasDefaultValue(0);
        _.Property(p => p.MaximumAllowableStock).IsRequired();
        _.Property(p => p.Stock).HasDefaultValue(0);
        _.HasOne(p => p.Category).WithMany(p => p.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.ClientNoAction);
        _.HasMany(p => p.EntryDocuments).WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.ClientNoAction);
        _.HasMany(p => p.SalesInvoices).WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.ClientNoAction);
    }
}