namespace Shared.Presentation;

public record SlidingAccountBalancesPresentation(params AccountBalancesByDatePresentation[] AccountBalancesByDate);

public record AccountBalancesByDatePresentation(
    DateOnly BalanceDate,
    params AccountBalancePresentation[] AccountBalances
);

public record AccountBalancePresentation(string AccountLabel, decimal Balance);