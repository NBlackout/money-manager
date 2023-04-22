using System.Xml.Serialization;

namespace MoneyManager.Infrastructure.Write.OfxProcessing;

public class OfxParser : IOfxParser
{
    public Task<AccountStatement> ExtractAccountStatement(Stream stream)
    {
        XmlSerializer serializer = new(typeof(Ofx));
        Ofx root = (Ofx)serializer.Deserialize(stream)!;

        Statement? statement = root.Bank?.StatementTransaction?.Statement;
        if (statement?.BankAccount?.BankIdentifier is null)
            throw CannotProcessOfxContent.DueToMissingBankIdentifierNode();
        if (statement.BankAccount?.AccountNumber is null)
            throw CannotProcessOfxContent.DueToMissingAccountNumberNode();
        if (statement.AvailableBalance is null || statement.AvailableBalance.Amount.HasValue is false)
            throw CannotProcessOfxContent.DueToMissingBalanceNode();

        decimal balance = statement.AvailableBalance.Amount.Value;

        return Task.FromResult(new AccountStatement(statement.BankAccount.BankIdentifier,
            statement.BankAccount.AccountNumber, balance));
    }

    [XmlRoot("OFX")]
    public class Ofx
    {
        [XmlElement("BANKMSGSRSV1")] public Bank? Bank { get; init; }
    }

    public class Bank
    {
        [XmlElement("STMTTRNRS")] public StatementTransaction? StatementTransaction { get; init; }
    }

    public class StatementTransaction
    {
        [XmlElement("STMTRS")] public Statement? Statement { get; init; }
    }

    public class Statement
    {
        [XmlElement("BANKACCTFROM")] public BankAccount? BankAccount { get; init; }
        [XmlElement("AVAILBAL")] public AvailableBalance? AvailableBalance { get; init; }
    }

    public class BankAccount
    {
        [XmlElement("BANKID")] public string? BankIdentifier { get; init; }
        [XmlElement("ACCTID")] public string? AccountNumber { get; init; }
    }

    public class AvailableBalance
    {
        [XmlElement("BALAMT")] public decimal? Amount { get; init; }
    }
}