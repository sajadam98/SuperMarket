
public class EntryDocument
{
    public int Id { get; set; }
    public DateOnly DateTime { get; set; }
    public DateOnly ManufactureDate { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public int Count { get; set; }
    public int PurchasePrice { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}