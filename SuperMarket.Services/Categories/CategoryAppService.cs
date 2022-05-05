public class CategoryAppService : CategoryService
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
        var isCategoryNameExist =
            _repository.IsCategoryNameExistDuringAddCategory(dto.Name);
        if (isCategoryNameExist)
        {
            throw new DuplicateCategoryNameInCategoryException();
        }

        var category = new Category
        {
            Name = dto.Name,
        };
        _repository.Add(category);
        _unitOfWork.Save();
    }

    public void Update(int id, UpdateCategoryDto dto)
    {
        var category = _repository.Find(id);
        if (category == null)
        {
            throw new CategoryNotExistException();
        }
        var isCategoryNameExist =
            _repository.IsCategoryNameExistDuringUpdateCategory(id,dto.Name);
        if (isCategoryNameExist)
        {
            throw new DuplicateCategoryNameInCategoryException();
        }

        category.Name = dto.Name;
        
        _repository.Update(category);
        _unitOfWork.Save();
    }
}