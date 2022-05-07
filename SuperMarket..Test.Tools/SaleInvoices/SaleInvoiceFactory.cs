
using System;

public class SaleInvoiceFactory
{
    public static AddSaleInvoiceDto GenerateAddSaleInvoiceDto(int productId)
    {
        return new AddSaleInvoiceDto
        {
            Count = 5,
            Price = 25000,
            ProductId = productId,
            BuyerName = "علی علینقیپور",
            DateTime = new DateTime(1900, 04, 16)
        };
    }
    
    public static SalesInvoice GenerateSaleInvoice(int productId)
    {
        return new SalesInvoice
        {
            Count = 50,
            Price = 18000,
            ProductId = productId,
            BuyerName = "علی علینقیپور",
            DateTime = new DateTime(1900, 04, 16)
        };
    }
}