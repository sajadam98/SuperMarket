using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/categories")]
public class CategoriesController : Controller
{
    private readonly CategoryService _service;

    public CategoriesController(CategoryService service)
    {
        _service = service;
    }

    [HttpPost]
    public void Add(AddCategoryDto dto)
    {
        _service.Add(dto);
    }

    [HttpPut("{id:int}")]
    public void Update(int id, UpdateCategoryDto dto)
    {
        _service.Update(id, dto);
    }

    [HttpDelete("{id:int}")]
    public void Delete(int id)
    {
        _service.Delete(id);
    }

    [HttpGet]
    public IList<GetCategoryDto> GetAll()
    {
        return _service.GetAll();
    }
}