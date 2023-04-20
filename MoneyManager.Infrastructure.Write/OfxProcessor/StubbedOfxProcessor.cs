namespace MoneyManager.Infrastructure.Write.OfxProcessor;

public class StubbedOfxProcessor : IOfxProcessor
{
    private readonly Dictionary<Stream, AccountId> results = new();

    public void SetResultFor(Stream stream, AccountId accountId) =>
        this.results[stream] = accountId;

    public Task<AccountId> Parse(Stream stream) =>
        Task.FromResult(this.results[stream]);
}