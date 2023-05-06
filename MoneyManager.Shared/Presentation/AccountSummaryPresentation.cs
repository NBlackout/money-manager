namespace MoneyManager.Shared.Presentation;

public record AccountSummaryPresentation(Guid Id, Guid BankId, string BankName, string Label, decimal Balance,
    DateTime BalanceDate, bool Tracked);