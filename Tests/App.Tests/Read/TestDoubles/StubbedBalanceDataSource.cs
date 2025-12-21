using App.Read.Ports;

namespace App.Tests.Read.TestDoubles;

public class StubbedBalanceDataSource : IBalanceDataSource
{
    private readonly Dictionary<DateOnly, decimal> data = [];

    public Task<decimal> On(DateOnly date) =>
        Task.FromResult(this.data[date]);

    public void Feed(DateOnly date, decimal balance) =>
        this.data[date] = balance;
}