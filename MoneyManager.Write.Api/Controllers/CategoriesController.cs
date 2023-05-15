namespace MoneyManager.Write.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private CreateCategory createCategory;

    public CategoriesController(CreateCategory createCategory)
    {
        this.createCategory = createCategory;
    }

    [HttpPost]
    public async Task Create(CategoryDto dto) =>
        await this.createCategory.Execute(dto.Id, dto.Label);
}