
using System;

public class UpdateEntryDocumentDtoBuilder
{
    private UpdateEntryDocumentDto _dto = new UpdateEntryDocumentDto
    {
        DateTime = new DateTime(1900, 04, 16),
        ManufactureDate = new DateTime(1900, 04, 16),
        ExpirationDate = new DateTime(1900, 10, 16),
        PurchasePrice = 18000
    };

    public UpdateEntryDocumentDtoBuilder WithCount(int count)
    {
        _dto.Count = count;
        return this;
    }

    public UpdateEntryDocumentDtoBuilder WithProductId(int productId)
    {
        _dto.ProductId = productId;
        return this;
    }

    public UpdateEntryDocumentDto Build()
    {
        return _dto;
    }
}