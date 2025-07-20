using App.Read.Ports;

namespace App.Tests.Read.TestDoubles;

public class StubbedAccountDetailsDataSource : IAccountDetailsDataSource
{
    private readonly Dictionary<Guid, AccountDetailsPresentation> dataSource = new();

    public Task<AccountDetailsPresentation> By(Guid id) =>
        Task.FromResult(this.dataSource[id]);

    public void Feed(Guid id, AccountDetailsPresentation details) =>
        this.dataSource[id] = details;
}