namespace MoneyManager.Shared.Presentation;

public record AccountSummaryPresentation(Guid Id, Guid BankId, string BankName, string Label, string Number, decimal Balance,
    DateTime BalanceDate, bool Tracked);