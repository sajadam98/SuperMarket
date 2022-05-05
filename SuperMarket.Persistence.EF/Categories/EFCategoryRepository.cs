﻿public class EFCategoryRepository : CategoryRepository
{
    private readonly EFDataContext _dbContext;

    public EFCategoryRepository(EFDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Category category)
    {
        _dbContext.Set<Category>().Add(category);
    }

    public bool IsCategoryNameExistDuringAddCategory(string name)
    {
        return _dbContext.Set<Category>().Any(_ => _.Name == name);
    }

    public Category Find(int id)
    {
        return _dbContext.Set<Category>().FirstOrDefault(_ => _.Id == id);
    }

    public void Update(Category category)
    {
        _dbContext.Set<Category>().Update(category);
    }

    public bool IsCategoryNameExistDuringUpdateCategory(int id,
        string name)
    {
        return _dbContext.Set<Category>().Where(_ => _.Id != id)
            .Any(_ => _.Name == name);
    }
}