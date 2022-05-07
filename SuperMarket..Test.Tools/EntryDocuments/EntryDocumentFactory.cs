using System;

namespace SuperMarket._Test.Tools.EntryDocuments;

public class EntryDocumentFactory
{
    public static AddEntryDocumentDto GenerateAddEntryDocumentDto(
        int productId)
    {
        return new AddEntryDocumentDto
        {
            Count = 50,
            ProductId = productId,
            DateTime = new DateTime(1900, 04, 16),
            ManufactureDate = new DateTime(1900, 04, 16),
            ExpirationDate = new DateTime(1900, 10, 16),
            PurchasePrice = 18000
        };
    }
    
    public static UpdateEntryDocumentDto GenerateUpdateEntryDocumentDto(
        int productId)
    {
        return new UpdateEntryDocumentDto
        {
            Count = 50,
            ProductId = productId,
            DateTime = new DateTime(1900, 04, 16),
            ManufactureDate = new DateTime(1900, 04, 16),
            ExpirationDate = new DateTime(1900, 10, 16),
            PurchasePrice = 18000
        };
    }

    public static EntryDocument GenerateEntryDocument(int productId = 1)
    {
        return new EntryDocument
        {
            Count = 50,
            ProductId = productId,
            DateTime = new DateTime(1900, 04, 16),
            ExpirationDate = new DateTime(1900, 10, 16),
            ManufactureDate = new DateTime(1900, 04, 16),
            PurchasePrice = 18000,
        };
    }
}