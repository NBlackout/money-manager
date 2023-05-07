namespace MoneyManager.Shared.Presentation;

public record AccountDetailsPresentation(Guid Id, string Label, decimal Balance,
    params TransactionSummary[] Transactions);