namespace Read.App.Ports;

public interface ISlidingBalancesDataSource
{
    Task<SlidingBalancesPresentation> All(DateOnly baseline, DateOnly startingFrom);
}

public record SlidingBalancesPresentation(params SlidingBalancePresentation[] SlidingBalances);

public record SlidingBalancePresentation(DateOnly BalanceDate, params AccountBalancePresentation[] AccountBalances);

public record AccountBalancePresentation(string AccountLabel, decimal Balance);