
public class GetEntryDocumentDto
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime ManufactureDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int Count { get; set; }
    public int PurchasePrice { get; set; }
    public int TotalPrice { get; set; }
    public GetProductDto Product { get; set; }
}