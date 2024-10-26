namespace Write.App.Ports;

public interface IBankStatementParser
{
    Task<AccountStatement> Extract(string fileName, Stream stream);
}

public record AccountStatement(ExternalId AccountNumber, Balance Balance, params TransactionStatement[] Transactions);

public record TransactionStatement(ExternalId Identifier, Amount Amount, Label Label, DateOnly Date, Label? Category);