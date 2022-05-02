using System.Data;
using FluentMigrator;

[Migration(202205020830)]
public class _202205020830_InitialTables : Migration
{
    public override void Up()
    {
        CreateCategoriesTable();
        CreateProductsTable();
        CreateEntryDocumentsTable();
        CreateSalesInvoicesTable();
    }
    
    public override void Down()
    {
        Delete.Table("EntryDocuments");
        Delete.Table("SalesInvoices");
        Delete.Table("Products");
        Delete.Table("Categories");
    }

    private void CreateSalesInvoicesTable()
    {
        Create.Table("SalesInvoices")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("DateTime").AsDateTime().WithDefaultValue(SystemMethods.CurrentDateTime)
            .WithColumn("BuyerName").AsString(100).NotNullable()
            .WithColumn("Count").AsInt32().NotNullable()
            .WithColumn("Price").AsInt32().NotNullable()
            .WithColumn("ProductId").AsInt32().NotNullable()
            .ForeignKey("FK_SalesInvoices_Products", "Products", "Id")
            .OnDelete(Rule.Cascade);
    }

    private void CreateEntryDocumentsTable()
    {
        Create.Table("EntryDocuments")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("DateTime").AsDateTime().WithDefaultValue(SystemMethods.CurrentDateTime)
            .WithColumn("ManufactureDate").AsDateTime().NotNullable()
            .WithColumn("ExpirationDate").AsDateTime().NotNullable()
            .WithColumn("Count").AsInt32().NotNullable()
            .WithColumn("PurchasePrice").AsInt32().NotNullable()
            .WithColumn("ProductId").AsInt32().NotNullable()
            .ForeignKey("FK_EntryDocuments_Products", "Products", "Id")
            .OnDelete(Rule.Cascade);
    }

    private void CreateProductsTable()
    {
        Create.Table("Products")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Name").AsString(70).NotNullable()
            .WithColumn("ProductKey").AsString(10).NotNullable()
            .WithColumn("Price").AsInt32().NotNullable()
            .WithColumn("Brand").AsString(50).NotNullable()
            .WithColumn("MinimumAllowableStock").AsInt32().WithDefaultValue(0)
            .WithColumn("MaximumAllowableStock").AsInt32().NotNullable()
            .WithColumn("Stock").AsInt32().WithDefaultValue(0)
            .WithColumn("CategoryId").AsInt32()
            .ForeignKey("FK_Products_Categories", "Categories", "Id")
            .OnDelete(Rule.Cascade);
    }

    private void CreateCategoriesTable()
    {
        Create.Table("Categories")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Name").AsString(50).NotNullable();
    }
}