namespace MoneyManager.Shared;

public record AccountSummary(Guid Id, string BankName, string Label, decimal Balance, bool Tracked);