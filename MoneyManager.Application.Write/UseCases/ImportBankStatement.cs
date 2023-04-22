namespace MoneyManager.Application.Write.UseCases;

public class ImportBankStatement
{
    private readonly IAccountRepository accountRepository;
    private readonly IOfxParser ofxParser;

    public ImportBankStatement(IAccountRepository accountRepository, IOfxParser ofxParser)
    {
        this.accountRepository = accountRepository;
        this.ofxParser = ofxParser;
    }

    public async Task Execute(Stream stream)
    {
        AccountStatement statement = await this.ofxParser.ExtractAccountStatement(stream);

        Account? account = await this.GetTrackedAccountDescribedBy(statement);
        if (account == null)
        {
            Guid id = await this.accountRepository.NextIdentity();
            account = statement.TrackDescribedAccount(id);
        }
        else
            account.Synchronize(statement.Balance);

        await this.accountRepository.Save(account);
    }

    private async Task<Account?> GetTrackedAccountDescribedBy(AccountStatement statement)
    {
        ExternalId externalId = new(statement.BankIdentifier, statement.AccountNumber);

        return await this.accountRepository.GetByExternalIdOrDefault(externalId);
    }
}