using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml.Serialization;
using Write.App.Model.ValueObjects;
using Write.Infra.Exceptions;

namespace Write.Infra.BankStatementParsing;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class OfxBankStatementParser
{
    public Task<AccountStatement> ExtractAccountStatement(Stream stream)
    {
        XmlSerializer serializer = new(typeof(Ofx));
        Ofx root = (Ofx)serializer.Deserialize(stream)!;

        StatementResponse statementResponse = root.BankMessageSetResponse!.StatementResponses.First();
        AvailableBalance? availableBalance = statementResponse.AvailableBalance;

        if (statementResponse.BankAccount?.BankIdentifier is null)
            throw CannotProcessOfxContent.DueToMissingBankIdentifierNode();
        if (statementResponse.BankAccount?.AccountNumber is null)
            throw CannotProcessOfxContent.DueToMissingAccountNumberNode();
        if (availableBalance is null)
            throw CannotProcessOfxContent.DueToMissingBalanceNode();

        TransactionStatement[] transactions = statementResponse.StatementTransactions
            .Select(t => new TransactionStatement(
                new ExternalId(t.Identifier),
                new Amount(t.Amount),
                new Label(t.Label),
                t.Date,
                null)
            )
            .ToArray();

        return Task.FromResult(
            new AccountStatement(
                new ExternalId(statementResponse.BankAccount.AccountNumber),
                new Balance(availableBalance.Amount, availableBalance.Date),
                transactions
            )
        );
    }

    [XmlRoot("OFX")]
    public class Ofx
    {
        [XmlElement("BANKMSGSRSV1")] public BankMessageSetResponse? BankMessageSetResponse { get; init; }
    }

    public class BankMessageSetResponse
    {
        [XmlArray("STMTTRNRS")]
        [XmlArrayItem("STMTRS")]
        public StatementResponse[] StatementResponses { get; init; } = [];
    }

    public class StatementResponse
    {
        [XmlElement("BANKACCTFROM")] public BankAccount? BankAccount { get; init; }
        [XmlElement("AVAILBAL")] public AvailableBalance? AvailableBalance { get; init; }

        [XmlArray("BANKTRANLIST")]
        [XmlArrayItem("STMTTRN")]
        public StatementTransaction[] StatementTransactions { get; init; } = [];
    }

    public class BankAccount
    {
        [XmlElement("BANKID")] public string? BankIdentifier { get; init; }
        [XmlElement("ACCTID")] public string? AccountNumber { get; init; }
    }

    public class AvailableBalance
    {
        [XmlElement("DTASOF")] public string RawDate { get; init; } = null!;
        [XmlElement("BALAMT")] public decimal Amount { get; init; }

        public DateOnly Date => DateOnly.ParseExact(this.RawDate, "yyyyMMddHHmmss", null);
    }

    public class StatementTransaction
    {
        [XmlElement("DTPOSTED")] public string RawDate { get; init; } = null!;
        [XmlElement("TRNAMT")] public string RawAmount { get; init; } = null!;
        [XmlElement("FITID")] public string Identifier { get; init; } = null!;
        [XmlElement("NAME")] public string Label { get; init; } = null!;

        public decimal Amount => decimal.Parse(this.RawAmount, CultureInfo.CreateSpecificCulture("fr-FR"));
        public DateOnly Date => DateOnly.ParseExact(this.RawDate, "yyyyMMdd", null);
    }
}