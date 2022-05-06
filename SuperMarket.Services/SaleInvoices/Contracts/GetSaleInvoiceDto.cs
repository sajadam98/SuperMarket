public class GetSaleInvoiceDto
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public string BuyerName { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public int TotalPrice { get; set; }
    public GetProductDto Product { get; set; }
}