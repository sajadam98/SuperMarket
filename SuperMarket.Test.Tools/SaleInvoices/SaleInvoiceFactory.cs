
using System;

public class SaleInvoiceFactory
{
    public static AddSaleInvoiceDto GenerateAddSaleInvoiceDto(int productId)
    {
        return new AddSaleInvoiceDto
        {
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
            Price = 24000,
            ProductId = productId,
            BuyerName = "علی علینقیپور",
            DateTime = new DateTime(1900, 04, 16)
        };
    }
}