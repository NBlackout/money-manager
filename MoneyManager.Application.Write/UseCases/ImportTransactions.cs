namespace MoneyManager.Application.Write.UseCases;

public class ImportTransactions
{
    private readonly IAccountRepository accountRepository;
    private readonly IOfxParser ofxParser;

    public ImportTransactions(IAccountRepository accountRepository, IOfxParser ofxParser)
    {
        this.accountRepository = accountRepository;
        this.ofxParser = ofxParser;
    }

    public async Task Execute(Stream stream)
    {
        AccountStatement accountStatement = await this.ofxParser.ExtractAccountId(stream);

        Account? account = await this.accountRepository.GetByIdOrDefault(accountStatement.ExternalId);
        if (account == null)
        {
            Guid id = await this.accountRepository.NextIdentity();
            account = accountStatement.Track(id);
        }
        else
            account.Synchronize(accountStatement.Balance);

        await this.accountRepository.Save(account);
    }
}