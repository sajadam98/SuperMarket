﻿public class CategoryAppService : CategoryService
{
    private readonly CategoryRepository _repository;
    private readonly UnitOfWork _unitOfWork;

    public CategoryAppService(CategoryRepository repository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public void Add(AddCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
        };
        _repository.Add(category);
        _unitOfWork.Save();
    }
}