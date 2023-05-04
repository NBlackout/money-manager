namespace MoneyManager.Write.Application.Model;

public record AccountSnapshot(Guid Id, Guid BankId, string Number, string Label, decimal Balance,
    bool Tracked);