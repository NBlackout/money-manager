using Client.Write.Infra.Gateways.Category;

namespace Client.Write.App.Tests.UseCases;

public class DeleteCategoryTests
{
    private readonly SpyCategoryGateway gateway = new();
    private readonly DeleteCategory sut;

    public DeleteCategoryTests()
    {
        this.sut = new DeleteCategory(this.gateway);
    }

    [Theory, RandomData]
    public async Task Should_delete_category(Guid id)
    {
        await this.sut.Execute(id);
        this.gateway.DeleteCalls.Should().Equal(id);
    }
}