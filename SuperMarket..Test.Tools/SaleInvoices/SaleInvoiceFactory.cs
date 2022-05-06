
using System;

public class SaleInvoiceFactory
{
    public static AddSaleInvoiceDto GenerateAddSaleInvoiceDto(int productId)
    {
        return new AddSaleInvoiceDto
        {
            Count = 50,
            Price = 18000,
            ProductId = productId,
            BuyerName = "علی علینقیپور",
            DateTime = new DateTime(1400, 04, 16)
        };
    }
}