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
        AccountStatement accountStatement = await this.ofxParser.ExtractAccountId(stream);

        Account? account = await this.GetAccountOrDefault(accountStatement);
        if (account == null)
        {
            Guid id = await this.accountRepository.NextIdentity();
            account = accountStatement.Track(id);
        }
        else
            account.Synchronize(accountStatement.Balance);

        await this.accountRepository.Save(account);
    }

    private async Task<Account?> GetAccountOrDefault(AccountStatement accountStatement)
    {
        ExternalId externalId = new(accountStatement.BankIdentifier, accountStatement.AccountNumber);

        return await this.accountRepository.GetByExternalIdOrDefault(externalId);
    }
}