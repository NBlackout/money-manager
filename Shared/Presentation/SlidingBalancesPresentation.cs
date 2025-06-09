namespace Shared.Presentation;

public record SlidingBalancesPresentation(params SlidingBalancePresentation[] SlidingBalances);

public record SlidingBalancePresentation(DateOnly BalanceDate, params AccountBalancePresentation[] AccountBalances);

public record AccountBalancePresentation(string AccountLabel, decimal Balance);