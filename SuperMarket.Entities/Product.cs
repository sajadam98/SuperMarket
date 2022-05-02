public class Product
{
    public Product()
    {
        EntryDocuments = new List<EntryDocument>();
        SalesInvoices = new List<SalesInvoice>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string ProductKey { get; set; }
    public int Price { get; set; }
    public string Brand { get; set; }
    public int MinimumAllowableStock { get; set; }
    public int MaximumAllowableStock { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public List<EntryDocument> EntryDocuments { get; set; }
    public List<SalesInvoice> SalesInvoices { get; set; }
}