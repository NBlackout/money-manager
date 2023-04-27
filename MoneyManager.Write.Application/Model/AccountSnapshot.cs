namespace MoneyManager.Write.Application.Model;

public record AccountSnapshot(Guid Id, string BankIdentifier, string Number, string Label, decimal Balance,
    bool Tracked);