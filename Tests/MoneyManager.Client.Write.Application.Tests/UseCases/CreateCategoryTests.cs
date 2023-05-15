using MoneyManager.Client.Write.Infrastructure.Gateways.Category;

namespace MoneyManager.Client.Write.Application.Tests.UseCases;

public class CreateCategoryTests
{
    private readonly SpyCategoryGateway gateway;
    private readonly CreateCategory sut;

    public CreateCategoryTests()
    {
        this.gateway = new SpyCategoryGateway();
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