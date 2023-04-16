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

    public async Task Handle(Stream stream)
    {
        AccountCharacteristics? accountDescription = await this.ofxParser.Process(stream);
        await this.accountRepository.Save(new Account(accountDescription.AccountNumber));
    }
}