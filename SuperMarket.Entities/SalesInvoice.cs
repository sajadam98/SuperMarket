public class SalesInvoice
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public string BuyerName { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}