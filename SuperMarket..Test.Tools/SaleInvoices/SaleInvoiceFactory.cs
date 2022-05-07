
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
    
    public static UpdateSaleInvoiceDto GenerateUpdatealeInvoiceDto(int productId)
    {
        return new UpdateSaleInvoiceDto
        {
            Count = 8,
            Price = 24000,
            ProductId = productId,
            BuyerName = "علی علینقیپور",
            DateTime = new DateTime(1900, 04, 16)
        };
    }
    
    public static SalesInvoice GenerateSaleInvoice(int productId = 1)
    {
        return new SalesInvoice
        {
            Count = 50,
            Price = 25000,
            ProductId = productId,
            BuyerName = "علی علینقیپور",
            DateTime = new DateTime(1900, 04, 16)
        };
    }
}