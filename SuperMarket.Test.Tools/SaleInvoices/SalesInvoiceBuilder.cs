using System;

public class SalesInvoiceBuilder
{
    private SalesInvoice _salesInvoice = new SalesInvoice
    {
        Price = 25000,
        BuyerName = "علی علینقیپور",
        DateTime = new DateTime(1900, 04, 16),
        Count = 2
    };

    public SalesInvoiceBuilder WithProductId(int productId)
    {
        _salesInvoice.ProductId = productId;
        return this;
    }

    public SalesInvoiceBuilder WithProduct(Product product)
    {
        _salesInvoice.Product = product;
        return this;
    }

    public SalesInvoiceBuilder WithPrice(int price)
    {
        _salesInvoice.Price = price;
        return this;
    }
    
    public SalesInvoiceBuilder WithCount(int count)
    {
        _salesInvoice.Count = count;
        return this;
    }

    public SalesInvoiceBuilder WithDateTime(DateTime dateTime)
    {
        _salesInvoice.DateTime = dateTime;
        return this;
    }
    
    public SalesInvoiceBuilder WithBuyerName(string buyerName)
    {
        _salesInvoice.BuyerName = buyerName;
        return this;
    }

    public SalesInvoice Build()
    {
        return _salesInvoice;
    }
}