using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class
    EntryDocumentEntityMap : IEntityTypeConfiguration<EntryDocument>
{
    public void Configure(EntityTypeBuilder<EntryDocument> _)
    {
        _.ToTable("EntryDocuments");
        _.HasKey(p => p.Id);
        _.Property(p => p.Id).ValueGeneratedOnAdd();
        _.Property(p => p.DateTime).HasDefaultValue(DateTime.Now.Date);
        _.Property(p => p.ManufactureDate).IsRequired();
        _.Property(p => p.ExpirationDate).IsRequired();
        _.Property(p => p.Count).IsRequired();
        _.Property(p => p.PurchasePrice).IsRequired();
        _.HasOne(p => p.Product).WithMany(p => p.EntryDocuments)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.ClientNoAction);
    }
}