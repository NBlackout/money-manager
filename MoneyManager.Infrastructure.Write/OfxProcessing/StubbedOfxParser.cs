namespace MoneyManager.Infrastructure.Write.OfxProcessing;

public class StubbedOfxParser : IOfxParser
{
    private readonly Dictionary<Stream, AccountStatement> results = new();

    public void SetResultFor(Stream stream, AccountStatement accountStatement) =>
        this.results[stream] = accountStatement;

    public Task<AccountStatement> ExtractAccountId(Stream stream) =>
        Task.FromResult(this.results[stream]);
}