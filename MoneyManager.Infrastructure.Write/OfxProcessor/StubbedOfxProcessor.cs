namespace MoneyManager.Infrastructure.Write.OfxProcessor;

public class StubbedOfxProcessor : IOfxProcessor
{
    private readonly Dictionary<Stream, AccountIdentification> results = new();

    public void SetResultFor(Stream stream, AccountIdentification accountIdentification) =>
        this.results[stream] = accountIdentification;

    public Task<AccountIdentification> Parse(Stream stream) =>
        Task.FromResult(this.results[stream]);
}