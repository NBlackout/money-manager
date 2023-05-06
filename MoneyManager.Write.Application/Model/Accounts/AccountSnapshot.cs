namespace MoneyManager.Write.Application.Model.Accounts;

public record AccountSnapshot(Guid Id, Guid BankId, string Number, string Label, decimal Balance, DateTime BalanceDate,
    bool Tracked);