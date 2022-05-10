using System;

public class UpdateSalesInvoiceDtoBuilder
{
    private UpdateSaleInvoiceDto _dto = new UpdateSaleInvoiceDto
    {
        Price = 25000,
        BuyerName = "علی علینقیپور",
        DateTime = new DateTime(1900, 04, 16)
    };

    public UpdateSalesInvoiceDtoBuilder WithProductId(int productId)
    {
        _dto.ProductId = productId;
        return this;
    }

    public UpdateSalesInvoiceDtoBuilder WithPrice(int price)
    {
        _dto.Price = price;
        return this;
    }

    public UpdateSalesInvoiceDtoBuilder WithCount(int count)
    {
        _dto.Count = count;
        return this;
    }

    public UpdateSalesInvoiceDtoBuilder WithDateTime(DateTime dateTime)
    {
        _dto.DateTime = dateTime;
        return this;
    }

    public UpdateSalesInvoiceDtoBuilder WithBuyerName(string buyerName)
    {
        _dto.BuyerName = buyerName;
        return this;
    }

    public UpdateSaleInvoiceDto Build()
    {
        return _dto;
    }
}