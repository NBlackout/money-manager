namespace Shared.Presentation;

public record AccountSummaryPresentation(Guid Id, Guid BankId, string Label, string Number, decimal Balance,
    DateTime BalanceDate);
