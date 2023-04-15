using MoneyManager.Application.Write.Model;
using MoneyManager.Application.Write.Ports;

namespace MoneyManager.Application.Write;

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
        var accountDescription = await ofxParser.Process(stream);
        await accountRepository.Save(new Account(accountDescription.AccountNumber));
    }
}