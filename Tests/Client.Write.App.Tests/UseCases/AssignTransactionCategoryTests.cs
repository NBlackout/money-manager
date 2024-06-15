using Client.Write.Infra.Gateways.Transaction;

namespace Client.Write.App.Tests.UseCases;

public class AssignTransactionCategoryTests
{
    private readonly SpyTransactionGateway gateway = new();
    private readonly AssignTransactionCategory sut;

    public AssignTransactionCategoryTests()
    {
        this.sut = new AssignTransactionCategory(this.gateway);
    }

    [Fact]
    public async Task Should_assign_transaction_category()
    {
        Guid transactionId = Guid.NewGuid();
        Guid categoryId = Guid.NewGuid();

        await this.sut.Execute(transactionId, categoryId);

        this.gateway.AssignCategoryCalls.Should().Equal((transactionId, categoryId));
    }
}