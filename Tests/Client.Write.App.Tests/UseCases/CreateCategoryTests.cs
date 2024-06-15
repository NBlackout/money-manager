using Client.Write.Infra.Gateways.Category;

namespace Client.Write.App.Tests.UseCases;

public class CreateCategoryTests
{
    private readonly SpyCategoryGateway gateway = new();
    private readonly CreateCategory sut;

    public CreateCategoryTests()
    {
        this.sut = new CreateCategory(this.gateway);
    }

    [Fact]
    public async Task Should_create_category()
    {
        Guid id = Guid.NewGuid();
        const string label = "Category label";

        await this.sut.Execute(id, label);

        this.gateway.Calls.Should().Equal((id, label));
    }
}