namespace Write.Infra.BankStatementParsing;

public class BankStatementParser(
    OfxBankStatementParser ofxBankStatementParser,
    CsvBankStatementParser csvBankStatementParser)
    : IBankStatementParser
{
    public Task<AccountStatement> Extract(string fileName, Stream stream)
    {
        return Path.GetExtension(fileName) switch
        {
            ".csv" => csvBankStatementParser.ExtractAccountStatement(stream),
            var _ => ofxBankStatementParser.ExtractAccountStatement(stream)
        };
    }
}