
public class EntryDocument
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime ManufactureDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int Count { get; set; }
    public int PurchasePrice { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}