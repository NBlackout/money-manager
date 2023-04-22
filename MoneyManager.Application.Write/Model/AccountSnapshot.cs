namespace MoneyManager.Application.Write.Model;

public record AccountSnapshot(Guid Id, string BankIdentifier, string Number, decimal Balance);