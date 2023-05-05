namespace MoneyManager.Shared.Presentation;

public record AccountSummary(Guid Id, Guid BankId, string BankName, string Label, decimal Balance, bool Tracked);