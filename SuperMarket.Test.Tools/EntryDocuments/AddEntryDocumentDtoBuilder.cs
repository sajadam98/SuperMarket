
using System;

public class AddEntryDocumentDtoBuilder
{
    private AddEntryDocumentDto _dto = new AddEntryDocumentDto
    {
        DateTime = new DateTime(1900, 04, 16),
        ManufactureDate = new DateTime(1900, 04, 16),
        ExpirationDate = new DateTime(1900, 10, 16),
        PurchasePrice = 18000
    };

    public AddEntryDocumentDtoBuilder WithProductId(int productId)
    {
        _dto.ProductId = productId;
        return this;
    }
    
    public AddEntryDocumentDtoBuilder WithCount(int count)
    {
        _dto.Count = count;
        return this;
    }

    public AddEntryDocumentDto Build()
    {
        return _dto;
    }
}