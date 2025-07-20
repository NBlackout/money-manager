using App.Write.Model.Accounts;
using App.Write.Model.Categories;
using App.Write.Model.Transactions;
using App.Write.Model.ValueObjects;
using App.Write.Ports;
using App.Write.UseCases;
using Infra.Write.BankStatementParsing;
using Infra.Write.Repositories;
using static App.Tests.Write.UseCases.ImportBankStatementTests.Data;
using TransactionSnapshot = App.Write.Model.Transactions.TransactionSnapshot;

namespace App.Tests.Write.UseCases;

public class ImportBankStatementTests
{
    private readonly InMemoryAccountRepository accountRepository = new();
    private readonly InMemoryCategoryRepository categoryRepository = new();
    private readonly InMemoryTransactionRepository transactionRepository = new();
    private readonly StubbedBankStatementParser bankStatementParser = new();
    private readonly ImportBankStatement sut;

    public ImportBankStatementTests()
    {
        this.sut = new ImportBankStatement(this.accountRepository, this.categoryRepository, this.transactionRepository, this.bankStatementParser);
    }

    [Theory]
    [RandomData]
    public async Task Tracks_unknown_bank_and_account(AccountSnapshot account)
    {
        this.FeedNextIdOf(account);
        this.Feed(AccountStatementFrom(account));

        await this.Verify(account with { Label = account.Number }, []);
    }

    [Theory]
    [RandomData]
    public async Task Tracks_unknown_account(AccountSnapshot account)
    {
        this.FeedNextIdOf(account);
        this.Feed(AccountStatementFrom(account));

        await this.Verify(account with { Label = account.Number }, []);
    }

    [Theory]
    [RandomData]
    public async Task Synchronizes_known_account(AccountSnapshot account)
    {
        TransactionSnapshot aTransaction = ATransactionFrom(account);
        TransactionSnapshot anotherTransaction = ATransactionFrom(account);

        this.Feed(account);
        this.FeedNextIdsOf(aTransaction, anotherTransaction);
        this.Feed(AccountStatementFrom(account, (aTransaction, null), (anotherTransaction, null)));

        await this.Verify(account, [], aTransaction, anotherTransaction);
    }

    [Theory]
    [RandomData]
    public async Task Assigns_existing_category_to_transactions(AccountSnapshot account, CategorySnapshot category)
    {
        TransactionSnapshot aTransaction = ATransactionFrom(account, category);
        TransactionSnapshot anotherTransaction = ATransactionFrom(account, category);

        this.Feed(account);
        this.Feed(category);
        this.FeedNextIdsOf(aTransaction, anotherTransaction);
        this.Feed(AccountStatementFrom(account, (aTransaction, category.Label), (anotherTransaction, category.Label)));

        await this.Verify(account, [category], aTransaction, anotherTransaction);
    }

    [Theory]
    [RandomData]
    public async Task Assigns_new_category_to_transactions(AccountSnapshot account, CategorySnapshot category)
    {
        TransactionSnapshot aTransaction = ATransactionFrom(account, category);
        TransactionSnapshot anotherTransaction = ATransactionFrom(account, category);

        this.Feed(account);
        this.FeedNextIdsOf(category);
        this.FeedNextIdsOf(aTransaction, anotherTransaction);
        this.Feed(AccountStatementFrom(account, (aTransaction, category.Label), (anotherTransaction, category.Label)));

        await this.Verify(account, [category with { Keywords = category.Label }], aTransaction, anotherTransaction);
    }

    private async Task Verify(AccountSnapshot expectedAccount, CategorySnapshot[] expectedCategories, params TransactionSnapshot[] expectedTransactions)
    {
        await this.sut.Execute(TheFileName, TheStream);

        Account actualAccount = await this.accountRepository.By(expectedAccount.Id);
        actualAccount.Snapshot.Should().Be(expectedAccount);

        foreach (CategorySnapshot expectedCategory in expectedCategories)
        {
            Category actualCategory = await this.categoryRepository.By(expectedCategory.Id);
            actualCategory.Snapshot.Should().Be(expectedCategory);
        }

        foreach (TransactionSnapshot expectedTransaction in expectedTransactions)
        {
            Transaction actualTransaction = await this.transactionRepository.By(expectedTransaction.Id);
            actualTransaction.Snapshot.Should().Be(expectedTransaction);
        }
    }

    private void Feed(AccountSnapshot account) =>
        this.accountRepository.FeedByExternalId(account.Number, account);

    private void Feed(CategorySnapshot category) =>
        this.categoryRepository.Feed(category);

    private void FeedNextIdOf(AccountSnapshot account) =>
        this.accountRepository.NextId = () => account.Id;

    private void FeedNextIdsOf(params CategorySnapshot[] categories)
    {
        int nextIdIndex = 0;
        this.categoryRepository.NextId = () => categories[nextIdIndex++].Id;
    }

    private void FeedNextIdsOf(params TransactionSnapshot[] transactions)
    {
        int nextIdIndex = 0;
        this.transactionRepository.NextId = () => transactions[nextIdIndex++].Id;
    }

    private void Feed(AccountStatement accountStatement) =>
        this.bankStatementParser.Feed(TheFileName, TheStream, accountStatement);

    internal static class Data
    {
        public const string TheFileName = "the filename";
        public static readonly MemoryStream TheStream = new([0xF0, 0x42]);

        public static TransactionSnapshot ATransactionFrom(AccountSnapshot account, CategorySnapshot? category = null) =>
            Any<TransactionSnapshot>() with { AccountId = account.Id, CategoryId = category?.Id };

        public static AccountStatement AccountStatementFrom(
            AccountSnapshot account,
            params (TransactionSnapshot Transaction, string? CategoryLabel)[] transactions) =>
            new(
                new ExternalId(account.Number),
                new Balance(account.Balance, account.BalanceDate),
                [
                    ..transactions.Select(t => new TransactionStatement(
                            new ExternalId(t.Transaction.ExternalId),
                            new Amount(t.Transaction.Amount),
                            new Label(t.Transaction.Label),
                            t.Transaction.Date,
                            Label.From(t.CategoryLabel)
                        )
                    )
                ]
            );
    }
}