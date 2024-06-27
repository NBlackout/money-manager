namespace Write.Infra.BankStatementParsing;

public class BankStatementParser : IBankStatementParser
{
    private readonly OfxBankStatementParser ofxBankStatementParser;

    public BankStatementParser(OfxBankStatementParser ofxBankStatementParser)
    {
        this.ofxBankStatementParser = ofxBankStatementParser;
    }

    public Task<AccountStatement> ExtractAccountStatement(Stream stream) =>
        this.ofxBankStatementParser.ExtractAccountStatement(stream);
}