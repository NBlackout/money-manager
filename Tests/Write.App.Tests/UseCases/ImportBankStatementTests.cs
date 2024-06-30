using Write.App.Model.Categories;
using Write.Infra.BankStatementParsing;
using static Shared.TestTooling.Randomizer;
using static Write.App.Tests.UseCases.ImportBankStatementTests.Data;

namespace Write.App.Tests.UseCases;

public class ImportBankStatementTests
{
    private readonly InMemoryBankRepository bankRepository = new();
    private readonly InMemoryAccountRepository accountRepository = new();
    private readonly InMemoryCategoryRepository categoryRepository = new();
    private readonly InMemoryTransactionRepository transactionRepository = new();
    private readonly StubbedBankStatementParser bankStatementParser = new();
    private readonly ImportBankStatement sut;

    public ImportBankStatementTests()
    {
        this.sut = new ImportBankStatement(this.bankRepository, this.accountRepository, this.categoryRepository,
            this.transactionRepository, this.bankStatementParser);
    }

    [Theory, RandomData]
    public async Task Should_track_unknown_bank_and_account(BankBuilder bank, AccountBuilder account)
    {
        this.FeedNextIdOf(bank);
        this.FeedNextIdOf(account);
        this.Feed(AccountStatementFrom(bank, account));

        await this.Verify(bank, account with { BankId = bank.Id, Label = account.Number }, []);
    }

    [Theory, RandomData]
    public async Task Should_track_unknown_account(BankBuilder bank, AccountBuilder account)
    {
        this.Feed(bank);
        this.FeedNextIdOf(account);
        this.Feed(AccountStatementFrom(bank, account));

        await this.Verify(bank, account with { BankId = bank.Id, Label = account.Number }, []);
    }

    [Theory, RandomData]
    public async Task Should_synchronize_known_account(BankBuilder bank)
    {
        AccountBuilder account = AnAccountFrom(bank);
        TransactionBuilder aTransaction = ATransactionFrom(account);
        TransactionBuilder anotherTransaction = ATransactionFrom(account);

        this.Feed(bank);
        this.Feed(account);
        this.FeedNextIdsOf(aTransaction, anotherTransaction);
        this.Feed(AccountStatementFrom(bank, account, aTransaction, anotherTransaction));

        await this.Verify(bank, account, [], aTransaction, anotherTransaction);
    }

    [Theory, RandomData]
    public async Task Should_assign_existing_category_to_transactions(BankBuilder bank)
    {
        AccountBuilder account = AnAccountFrom(bank);
        CategoryBuilder category = Any<CategoryBuilder>();
        TransactionBuilder aTransaction = ATransactionFrom(account, category);
        TransactionBuilder anotherTransaction = ATransactionFrom(account, category);

        this.Feed(bank);
        this.Feed(account);
        this.Feed(category);
        this.FeedNextIdsOf(aTransaction, anotherTransaction);
        this.Feed(AccountStatementFrom(bank, account, aTransaction, anotherTransaction));

        await this.Verify(bank, account, [], aTransaction, anotherTransaction);
    }

    private async Task Verify(BankBuilder expectedBank, AccountBuilder expectedAccount,
        CategoryBuilder[] expectedCategories, params TransactionBuilder[] expectedTransactions)
    {
        await this.sut.Execute(TheFileName, TheStream);

        Bank actualBank = await this.bankRepository.By(expectedBank.Id);
        actualBank.Snapshot.Should().Be(expectedBank.ToSnapshot());

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

    private void Feed(BankBuilder bank) =>
        this.bankRepository.FeedByExternalId(bank.ExternalId, bank.Build());

    private void Feed(AccountBuilder account) =>
        this.accountRepository.FeedByExternalId(new ExternalId(account.BankId, account.Number), account.Build());

    private void Feed(CategoryBuilder category) =>
        this.categoryRepository.Feed(category.Build());

    private void FeedNextIdOf(BankBuilder bank) =>
        this.bankRepository.NextId = () => bank.Id;

    private void FeedNextIdOf(AccountBuilder account) =>
        this.accountRepository.NextId = () => account.Id;

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

        public static AccountBuilder AnAccountFrom(BankBuilder bank) =>
            Any<AccountBuilder>() with { BankId = bank.Id };

        public static TransactionBuilder ATransactionFrom(AccountBuilder account, CategoryBuilder? category = null) =>
            Any<TransactionBuilder>() with
            {
                AccountId = account.Id, CategoryId = category?.Id, CategoryLabel = category?.Label
            };

        public static AccountStatement AccountStatementFrom(BankBuilder bank, AccountBuilder account,
            params TransactionBuilder[] transactions)
        {
            return new AccountStatement(bank.ExternalId, account.Number, account.Balance, account.BalanceDate,
                transactions
                    .Select(t => new TransactionStatement(t.ExternalId, t.Amount, t.Label, t.Date, t.CategoryLabel))
                    .ToArray()
            );
        }
    }
}
