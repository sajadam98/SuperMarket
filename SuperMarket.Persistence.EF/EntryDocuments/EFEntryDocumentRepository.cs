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
}