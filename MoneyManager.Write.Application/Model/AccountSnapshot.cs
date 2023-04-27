namespace MoneyManager.Write.Application.Model;

public record AccountSnapshot(Guid Id, string BankIdentifier, string Number, decimal Balance, bool Tracked);