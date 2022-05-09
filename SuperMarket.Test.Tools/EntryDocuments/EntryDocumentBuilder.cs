using System;

public class EntryDocumentBuilder
{
    private EntryDocument _entryDocument = new EntryDocument
    {
        DateTime = new DateTime(1900, 04, 16),
        ExpirationDate = new DateTime(1900, 10, 16),
        ManufactureDate = new DateTime(1900, 04, 16),
        PurchasePrice = 18000,
    };

    public EntryDocumentBuilder WithProductId(int productId)
    {
        _entryDocument.ProductId = productId;
        return this;
    }

    public EntryDocumentBuilder WithProduct(Product product)
    {
        _entryDocument.Product = product;
        return this;
    }

    public EntryDocumentBuilder WithCount(int count)
    {
        _entryDocument.Count = count;
        return this;
    }
    
    public EntryDocumentBuilder WithDateTime(DateTime dateTime)
    {
        _entryDocument.DateTime = dateTime;
        return this;
    }
    
    public EntryDocumentBuilder WithExpirationDate(DateTime expirationDate)
    {
        _entryDocument.ExpirationDate = expirationDate;
        return this;
    }
    
    public EntryDocumentBuilder WithManufactureDate(DateTime manufactureDate)
    {
        _entryDocument.ManufactureDate = manufactureDate;
        return this;
    }
    
    public EntryDocumentBuilder WithPurchasePrice(int purchasePrice)
    {
        _entryDocument.PurchasePrice = purchasePrice;
        return this;
    }

    public EntryDocument Build()
    {
        return _entryDocument;
    }
}