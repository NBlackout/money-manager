namespace Write.App.Model.Accounts;

public record AccountSnapshot(Guid Id, string Number, string Label, decimal Balance, DateTime BalanceDate);
