namespace MoneyManager.Client.Read.Infrastructure.Gateways;

public class StubbedAccountDetailsGateway : IAccountDetailsGateway
{
    private readonly Dictionary<Guid, AccountDetailsPresentation> data = new();

    public Task<AccountDetailsPresentation> Get(Guid id) =>
        Task.FromResult(this.data[id]);

    public void Feed(Guid id, AccountDetailsPresentation expected) =>
        this.data[id] = expected;
}