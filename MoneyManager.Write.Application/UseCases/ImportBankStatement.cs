namespace MoneyManager.Write.Application.UseCases;

public class ImportBankStatement
{
    private readonly IBankRepository bankRepository;
    private readonly IAccountRepository accountRepository;
    private readonly IOfxParser ofxParser;

    public ImportBankStatement(IBankRepository bankRepository, IAccountRepository accountRepository,
        IOfxParser ofxParser)
    {
        this.bankRepository = bankRepository;
        this.accountRepository = accountRepository;
        this.ofxParser = ofxParser;
    }

    public async Task Execute(Stream stream)
    {
        AccountStatement statement = await this.ofxParser.ExtractAccountStatement(stream);

        Bank? bank = await this.bankRepository.GetByExternalIdOrDefault(statement.BankIdentifier);
        if (bank == null)
        {
            Guid id = await this.bankRepository.NextIdentity();
            bank = statement.TrackDescribedBank(id);
        }

        Account? account = await this.accountRepository.GetByExternalIdOrDefault(new ExternalId(bank.Id, statement.AccountNumber));
        if (account == null)
        {
            Guid id = await this.accountRepository.NextIdentity();
            account = bank.TrackAccount(id, statement.AccountNumber, statement.Balance, statement.BalanceDate);
        }
        else
            account.Synchronize(statement.Balance, statement.BalanceDate);

        await this.bankRepository.Save(bank);
        await this.accountRepository.Save(account);
    }
}