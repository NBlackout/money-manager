namespace App.Read.Ports;

public interface IBalanceDataSource
{
    Task<decimal> On(DateOnly date);
}