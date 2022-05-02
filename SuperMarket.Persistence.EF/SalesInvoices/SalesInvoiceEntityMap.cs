using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SalesInvoiceEntityMap : IEntityTypeConfiguration<SalesInvoice>
{
    public void Configure(EntityTypeBuilder<SalesInvoice> _)
    {
        _.ToTable("SalesInvoices");
        _.HasKey(p => p.Id);
        _.Property(p => p.Id).ValueGeneratedOnAdd();
        _.Property(p => p.DateTime).HasDefaultValue(DateTime.Now.Date);
        _.Property(p => p.BuyerName).HasMaxLength(100).IsRequired();
        _.Property(p => p.Count).IsRequired();
        _.Property(p => p.Price).IsRequired();
        _.HasOne(p => p.Product).WithMany(s => s.SalesInvoices)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.ClientNoAction);
    }
}