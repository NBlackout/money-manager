namespace Write.App.Model.Accounts;

public record AccountSnapshot(Guid Id, Guid BankId, string Number, string Label, decimal Balance, DateTime BalanceDate);
