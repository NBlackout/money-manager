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
        AccountIdentification accountIdentification = await this.ofxProcessor.Parse(stream);

        bool alreadyExists = await this.accountRepository.ExistsByNumber(accountIdentification.Number);
        if (alreadyExists)
            return;

        Guid id = await this.accountRepository.NextIdentity();
        Account account = accountIdentification.Track(id);
        await this.accountRepository.Save(account);
    }
}