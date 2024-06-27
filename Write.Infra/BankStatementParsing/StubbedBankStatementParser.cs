namespace Write.Infra.BankStatementParsing;

public class StubbedBankStatementParser : IBankStatementParser
{
    private readonly Dictionary<Stream, AccountStatement> results = new();

    public void SetAccountStatementFor(Stream stream, AccountStatement accountStatement) =>
        this.results[stream] = accountStatement;

    public Task<AccountStatement> ExtractAccountStatement(Stream stream) =>
        Task.FromResult(this.results[stream]);
}