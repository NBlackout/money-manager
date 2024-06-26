namespace Write.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private CreateCategory createCategory;
    private DeleteCategory deleteCategory;

    public CategoriesController(CreateCategory createCategory, DeleteCategory deleteCategory)
    {
        this.createCategory = createCategory;
        this.deleteCategory = deleteCategory;
    }

    [HttpPost]
    public async Task Create(CategoryDto dto) =>
        await this.createCategory.Execute(dto.Id, dto.Label, dto.Keywords);
    
    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id)=>
        await this.deleteCategory.Execute(id);
}