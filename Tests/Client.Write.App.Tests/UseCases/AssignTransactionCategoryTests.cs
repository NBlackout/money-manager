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

    [Theory, RandomData]
    public async Task Should_assign_transaction_category(Guid transactionId, Guid categoryId)
    {
        await this.sut.Execute(transactionId, categoryId);
        this.gateway.AssignCategoryCalls.Should().Equal((transactionId, categoryId));
    }
}