public class EFUnitOfWork : UnitOfWork
{
    private readonly EFDataContext _dbContext;

    public EFUnitOfWork(EFDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Save()
    {
        _dbContext.SaveChanges();
    }
}