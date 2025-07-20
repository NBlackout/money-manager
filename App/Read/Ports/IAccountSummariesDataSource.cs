namespace App.Read.Ports;

public interface IAccountSummariesDataSource
{
    Task<AccountSummaryPresentation[]> All();
}

public record AccountSummaryPresentation(Guid Id, string Label, string Number, decimal Balance);