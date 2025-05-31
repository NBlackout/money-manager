using Client.Write.App.Tests.TestDoubles;

namespace Client.Write.App.Tests.UseCases;

public class CreateCategoryTests
{
    private readonly SpyCategoryGateway gateway = new();
    private readonly CreateCategory sut;

    public CreateCategoryTests()
    {
        this.sut = new CreateCategory(this.gateway);
    }

    [Theory]
    [RandomData]
    public async Task Creates_category(Guid id, string label, string keywords)
    {
        await this.sut.Execute(id, label, keywords);
        this.gateway.CreateCalls.Should().Equal((id, label, keywords));
    }
}