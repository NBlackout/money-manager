namespace App.Read.Ports;

public interface IAccountDetailsDataSource
{
    Task<AccountDetailsPresentation> By(Guid id);
}

public record AccountDetailsPresentation(Guid Id, string Label, string Number, decimal Balance, DateOnly BalanceDate);