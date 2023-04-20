namespace MoneyManager.Application.Write.UseCases;

public class ImportTransactions
{
    private readonly IAccountRepository accountRepository;
    private readonly IOfxProcessor ofxProcessor;

    public ImportTransactions(IAccountRepository accountRepository, IOfxProcessor ofxProcessor)
    {
        this.accountRepository = accountRepository;
        this.ofxProcessor = ofxProcessor;
    }

    public async Task Execute(Stream stream)
    {
        AccountId accountId = await this.ofxProcessor.Parse(stream);

        bool alreadyExists = await this.accountRepository.Exists(accountId);
        if (alreadyExists)
            return;

        Account account = accountId.Track();
        await this.accountRepository.Save(account);
    }
}