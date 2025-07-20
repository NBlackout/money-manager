namespace App.Write.Model.Accounts;

public record AccountSnapshot(AccountId Id, string Number, string Label, decimal Balance, DateOnly BalanceDate);