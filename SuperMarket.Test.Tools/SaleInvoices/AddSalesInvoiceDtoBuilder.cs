using System;

public class AddSalesInvoiceDtoBuilder
{
    private AddSaleInvoiceDto _dto = new AddSaleInvoiceDto
    {
        Price = 25000,
        BuyerName = "علی علینقیپور",
        DateTime = new DateTime(1900, 04, 16)
    }; 
    
    public AddSalesInvoiceDtoBuilder WithProductId(int productId)
    {
        _dto.ProductId = productId;
        return this;
    }

    public AddSalesInvoiceDtoBuilder WithPrice(int price)
    {
        _dto.Price = price;
        return this;
    }
    
    public AddSalesInvoiceDtoBuilder WithCount(int count)
    {
        _dto.Count = count;
        return this;
    }

    public AddSalesInvoiceDtoBuilder WithDateTime(DateTime dateTime)
    {
        _dto.DateTime = dateTime;
        return this;
    }
    
    public AddSalesInvoiceDtoBuilder WithBuyerName(string buyerName)
    {
        _dto.BuyerName = buyerName;
        return this;
    }

    public AddSaleInvoiceDto Build()
    {
        return _dto;
    }
}