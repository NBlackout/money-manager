using Write.App.Model.Categories;
using Write.Infra.BankStatementParsing;
using static Shared.TestTooling.Randomizer;
using static Write.App.Tests.UseCases.ImportBankStatementTests.Data;

namespace Write.App.Tests.UseCases;

public class ImportBankStatementTests
{
    private readonly InMemoryAccountRepository accountRepository = new();
    private readonly InMemoryCategoryRepository categoryRepository = new();
    private readonly InMemoryTransactionRepository transactionRepository = new();
    private readonly StubbedBankStatementParser bankStatementParser = new();
    private readonly ImportBankStatement sut;

    public ImportBankStatementTests()
    {
        this.sut = new ImportBankStatement(this.accountRepository, this.categoryRepository,
            this.transactionRepository, this.bankStatementParser);
    }

    [Theory, RandomData]
    public async Task Should_track_unknown_bank_and_account(AccountBuilder account)
    {
        this.FeedNextIdOf(account);
        this.Feed(AccountStatementFrom(account));

        await this.Verify(account with { Label = account.Number }, []);
    }

    [Theory, RandomData]
    public async Task Should_track_unknown_account(AccountBuilder account)
    {
        this.FeedNextIdOf(account);
        this.Feed(AccountStatementFrom(account));

        await this.Verify(account with { Label = account.Number }, []);
    }

    [Theory, RandomData]
    public async Task Should_synchronize_known_account(AccountBuilder account)
    {
        TransactionBuilder aTransaction = ATransactionFrom(account);
        TransactionBuilder anotherTransaction = ATransactionFrom(account);

        this.Feed(account);
        this.FeedNextIdsOf(aTransaction, anotherTransaction);
        this.Feed(AccountStatementFrom(account, aTransaction, anotherTransaction));

        await this.Verify(account, [], aTransaction, anotherTransaction);
    }

    [Theory, RandomData]
    public async Task Should_assign_existing_category_to_transactions(AccountBuilder account, CategoryBuilder category)
    {
        TransactionBuilder aTransaction = ATransactionFrom(account, category);
        TransactionBuilder anotherTransaction = ATransactionFrom(account, category);

        this.Feed(account);
        this.Feed(category);
        this.FeedNextIdsOf(aTransaction, anotherTransaction);
        this.Feed(AccountStatementFrom(account, aTransaction, anotherTransaction));

        await this.Verify(account, [category], aTransaction, anotherTransaction);
    }

    [Theory, RandomData]
    public async Task Should_assign_new_category_to_transactions(AccountBuilder account, CategoryBuilder category)
    {
        TransactionBuilder aTransaction = ATransactionFrom(account, category);
        TransactionBuilder anotherTransaction = ATransactionFrom(account, category);

        this.Feed(account);
        this.FeedNextIdsOf(category);
        this.FeedNextIdsOf(aTransaction, anotherTransaction);
        this.Feed(AccountStatementFrom(account, aTransaction, anotherTransaction));

        await this.Verify(account, [category with { Keywords = category.Label }], aTransaction,
            anotherTransaction);
    }

    private async Task Verify(AccountBuilder expectedAccount, CategoryBuilder[] expectedCategories,
        params TransactionBuilder[] expectedTransactions)
    {
        await this.sut.Execute(TheFileName, TheStream);

        Account actualAccount = await this.accountRepository.By(expectedAccount.Id);
        actualAccount.Snapshot.Should().Be(expectedAccount.ToSnapshot());

        foreach (CategoryBuilder expectedCategory in expectedCategories)
        {
            Category actualCategory = await this.categoryRepository.By(expectedCategory.Id);
            actualCategory.Snapshot.Should().Be(expectedCategory.ToSnapshot());
        }

        foreach (TransactionBuilder expectedTransaction in expectedTransactions)
        {
            Transaction actualTransaction = await this.transactionRepository.By(expectedTransaction.Id);
            actualTransaction.Snapshot.Should().Be(expectedTransaction.ToSnapshot());
        }
    }

    private void Feed(AccountBuilder account) =>
        this.accountRepository.FeedByExternalId(account.Number, account.Build());

    private void Feed(CategoryBuilder category) =>
        this.categoryRepository.Feed(category.Build());

    private void FeedNextIdOf(AccountBuilder account) =>
        this.accountRepository.NextId = () => account.Id;

    private void FeedNextIdsOf(params CategoryBuilder[] categories)
    {
        int nextIdIndex = 0;
        this.categoryRepository.NextId = () => categories[nextIdIndex++].Id;
    }

    private void FeedNextIdsOf(params TransactionBuilder[] transactions)
    {
        int nextIdIndex = 0;
        this.transactionRepository.NextId = () => transactions[nextIdIndex++].Id;
    }

    private void Feed(AccountStatement accountStatement) =>
        this.bankStatementParser.Feed(TheFileName, TheStream, accountStatement);

    internal static class Data
    {
        public static readonly string TheFileName = "the filename";
        public static readonly MemoryStream TheStream = new([0xF0, 0x42]);

        public static TransactionBuilder ATransactionFrom(AccountBuilder account, CategoryBuilder? category = null) =>
            Any<TransactionBuilder>() with
            {
                AccountId = account.Id, CategoryId = category?.Id, CategoryLabel = category?.Label
            };

        public static AccountStatement AccountStatementFrom(AccountBuilder account,
            params TransactionBuilder[] transactions)
        {
            return new AccountStatement(account.Number, account.Balance, account.BalanceDate, transactions
                .Select(t => new TransactionStatement(t.ExternalId, t.Amount, t.Label, t.Date, t.CategoryLabel))
                .ToArray()
            );
        }
    }
}
