public class EFEntryDocumentRepository : EntryDocumentRepository
{
    private readonly EFDataContext _dbContext;

    public EFEntryDocumentRepository(EFDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(EntryDocument entryDocument)
    {
        _dbContext.Set<EntryDocument>().Add(entryDocument);
    }

    public IList<GetEntryDocumentDto> GetAll()
    {
        return _dbContext.Set<EntryDocument>()
            .Select(_ => new GetEntryDocumentDto
            {
                Count = _.Count,
                Id = _.Id,
                Product = new GetProductDto
                {
                    Brand = _.Product.Brand,
                    Id = _.Product.Id,
                    Name = _.Product.Name,
                    Price = _.Product.Price,
                    Stock = _.Product.Stock,
                    ProductKey = _.Product.ProductKey,
                    MaximumAllowableStock =
                        _.Product.MaximumAllowableStock,
                    MinimumAllowableStock = _.Product.MinimumAllowableStock
                },
                DateTime = _.DateTime,
                ExpirationDate = _.ExpirationDate,
                ManufactureDate = _.ManufactureDate,
                PurchasePrice = _.PurchasePrice,
                TotalPrice = _.Count * _.PurchasePrice
            }).ToList();
    }

    public int GetTotalPurchase()
    {
        var totalPrice = 0;
        var entryDocumentsPrice = _dbContext.Set<EntryDocument>().Select(_ => _.Count * _.PurchasePrice);
        foreach (var item in entryDocumentsPrice)
        {
            totalPrice += item;
        }

        return totalPrice;
    }
}