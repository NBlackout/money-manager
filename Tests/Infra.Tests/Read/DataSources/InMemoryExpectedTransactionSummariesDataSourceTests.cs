using App.Read.Ports;
using App.Tests.Read.Tooling;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;

namespace Infra.Tests.Read.DataSources;

public class InMemoryExpectedTransactionSummariesDataSourceTests
    : InfraTest<IExpectedTransactionSummariesDataSource, InMemoryExpectedTransactionSummariesDataSource>
{
    private readonly InMemoryRecurringTransactionRepository recurringTransactionRepository;
    private readonly InMemoryCategoryRepository categoryRepository;

    public InMemoryExpectedTransactionSummariesDataSourceTests()
    {
        this.recurringTransactionRepository = this.Resolve<InMemoryRecurringTransactionRepository, InMemoryRecurringTransactionRepository>();
        this.categoryRepository = this.Resolve<InMemoryCategoryRepository, InMemoryCategoryRepository>();
    }

    [Fact]
    public async Task Gives_recurring_transactions_of_month()
    {
        RecurringTransactionBuilder aRecurringTransaction = ARecurringTransaction() with { Date = DateOnly.Parse("2024-08-12") };
        RecurringTransactionBuilder anotherRecurringTransaction = ARecurringTransaction() with { Date = DateOnly.Parse("2024-08-28") };
        this.Feed(aRecurringTransaction, anotherRecurringTransaction);
        await this.Verify(2024, 08, ExpectedFrom(aRecurringTransaction, anotherRecurringTransaction));
    }

    [Theory, RandomData]
    public async Task Gives_category(CategoryBuilder category)
    {
        RecurringTransactionBuilder recurringTransaction = ARecurringTransaction() with { Category = category };
        this.Feed(recurringTransaction);
        await this.Verify(ExpectedFrom(recurringTransaction) with { CategoryLabel = category.Label });
    }

    [Theory]
    [RandomData]
    public async Task Excludes_transactions_of_another_period(RecurringTransactionBuilder recurringTransaction)
    {
        this.Feed(recurringTransaction);
        await this.Verify(AnythingBut(recurringTransaction.Date.Year), recurringTransaction.Date.Month);
        await this.Verify(recurringTransaction.Date.Year, AnythingBut(recurringTransaction.Date.Month));
    }

    private async Task Verify(ExpectedTransactionSummaryPresentation expected) =>
        await this.Verify(expected.Date.Year, expected.Date.Month, expected);

    private async Task Verify(int year, int month, params ExpectedTransactionSummaryPresentation[] expected)
    {
        ExpectedTransactionSummaryPresentation[] actual = await this.Sut.By(year, month);
        actual.Should().Equal(expected);
    }

    private void Feed(params RecurringTransactionBuilder[] expected)
    {
        this.recurringTransactionRepository.Feed([..expected.Select(t => t.ToSnapshot())]);
        this.categoryRepository.Feed([..expected.Where(t => t.Category is not null).Select(t => t.Category!.ToSnapshot())]);
    }

    private static RecurringTransactionBuilder ARecurringTransaction() =>
        BuilderHelpers.ARecurringTransaction() with { Category = null };

    private static ExpectedTransactionSummaryPresentation[] ExpectedFrom(params RecurringTransactionBuilder[] recurringTransaction) =>
        recurringTransaction.Select(ExpectedFrom).ToArray();

    private static ExpectedTransactionSummaryPresentation ExpectedFrom(RecurringTransactionBuilder recurringTransaction) =>
        recurringTransaction.ToSummary();
}