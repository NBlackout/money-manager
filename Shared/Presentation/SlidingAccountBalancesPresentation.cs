namespace Shared.Presentation;

public record SlidingAccountBalancesPresentation(AccountBalancesByDatePresentation[] AccountBalancesByDate);

public record AccountBalancesByDatePresentation(DateOnly BalanceDate, AccountBalancePresentation[] AccountBalances);

public record AccountBalancePresentation(string AccountLabel, decimal Balance);
