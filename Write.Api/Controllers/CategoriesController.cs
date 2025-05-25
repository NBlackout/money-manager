using Write.App.Model.Categories;
using Write.App.Model.ValueObjects;

namespace Write.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(CreateCategory createCategory, DeleteCategory deleteCategory) : ControllerBase
{
    [HttpPost]
    public async Task Create(CategoryDto dto) =>
        await createCategory.Execute(new CategoryId(dto.Id), new Label(dto.Label), dto.Keywords);

    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id) =>
        await deleteCategory.Execute(new CategoryId(id));
}