namespace Write.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(CreateCategory createCategory, DeleteCategory deleteCategory) : ControllerBase
{
    [HttpPost]
    public async Task Create(CategoryDto dto) =>
        await createCategory.Execute(dto.Id, dto.Label, dto.Keywords);

    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id) =>
        await deleteCategory.Execute(id);
}