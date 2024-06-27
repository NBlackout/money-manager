namespace Write.Infra.BankStatementParsing;

public class StubbedBankStatementParser : IBankStatementParser
{
    private readonly Dictionary<(string, Stream), AccountStatement> results = new();

    public void SetAccountStatementFor(string fileName, Stream stream, AccountStatement accountStatement) =>
        this.results[(fileName, stream)] = accountStatement;

    public Task<AccountStatement> ExtractAccountStatement(string fileName, Stream stream) =>
        Task.FromResult(this.results[(fileName, stream)]);
}