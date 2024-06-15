namespace Read.Infra.DataSources.AccountDetails;

public class StubbedAccountDetailsDataSource : IAccountDetailsDataSource
{
    private readonly Dictionary<Guid, AccountDetailsPresentation> dataSource = new();

    public Task<AccountDetailsPresentation> Get(Guid id) =>
        Task.FromResult(this.dataSource[id]);

    public void Feed(Guid id, AccountDetailsPresentation expected) =>
        this.dataSource[id] = expected;
}