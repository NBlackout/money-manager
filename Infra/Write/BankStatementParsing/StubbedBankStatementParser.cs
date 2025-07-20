using App.Write.Ports;

namespace Infra.Write.BankStatementParsing;

public class StubbedBankStatementParser : IBankStatementParser
{
    private readonly Dictionary<(string, Stream), AccountStatement> results = new();

    public Task<AccountStatement> Extract(string fileName, Stream stream) =>
        Task.FromResult(this.results[(fileName, stream)]);

    public void Feed(string fileName, Stream stream, AccountStatement accountStatement) =>
        this.results[(fileName, stream)] = accountStatement;
}