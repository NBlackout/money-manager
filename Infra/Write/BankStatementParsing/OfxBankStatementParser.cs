using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Serialization;
using App.Write.Model.Accounts;
using App.Write.Model.ValueObjects;
using App.Write.Ports;
using Infra.Write.Exceptions;

namespace Infra.Write.BankStatementParsing;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class OfxBankStatementParser
{
    public async Task<AccountStatement> ExtractAccountStatement(Stream stream)
    {
        Ofx root = await Deserialize(stream);
        StatementResponse statementResponse = root.BankMessageSetResponse!.StatementResponses.First();
        BankAccount? bankAccount = statementResponse.BankAccount;
        BankAccountBalance? availableBalance = statementResponse.AvailableBalance;
        BankAccountBalance? ledgerBalance = statementResponse.LedgerBalance;

        if (bankAccount?.BankIdentifier is null)
            throw CannotProcessOfxContent.DueToMissingBankIdentifierNode();
        if (bankAccount.AccountNumber is null)
            throw CannotProcessOfxContent.DueToMissingAccountNumberNode();
        if (availableBalance is null && ledgerBalance is null)
            throw CannotProcessOfxContent.DueToMissingBalanceNode();

        BankAccountBalance balance = (availableBalance ?? ledgerBalance)!;
        TransactionStatement[] transactions =
        [
            ..statementResponse.StatementTransactions.Select(t => new TransactionStatement(
                    new ExternalId(t.Identifier),
                    new Amount(t.Amount),
                    new Label(t.Label),
                    t.Date,
                    null
                )
            )
        ];

        return new AccountStatement(new ExternalId(bankAccount.AccountNumber), new Balance(balance.Amount, balance.Date), transactions);
    }

    private static async Task<Ofx> Deserialize(Stream stream)
    {
        Stream sanitized = await Sanitize(stream);
        XmlSerializer serializer = new(typeof(Ofx));

        return (Ofx)serializer.Deserialize(sanitized)!;
    }

    private static async Task<Stream> Sanitize(Stream stream)
    {
        using StreamReader reader = new(stream);
        string text = await reader.ReadToEndAsync();
        int ofxTagPosition = text.IndexOf("<OFX>", StringComparison.InvariantCulture);

        return new MemoryStream(Encoding.UTF8.GetBytes(text[ofxTagPosition..]));
    }

    [XmlRoot("OFX")]
    public class Ofx
    {
        [XmlElement("BANKMSGSRSV1")] public BankMessageSetResponse? BankMessageSetResponse { get; init; }
    }

    public class BankMessageSetResponse
    {
        [XmlArray("STMTTRNRS")] [XmlArrayItem("STMTRS")] public StatementResponse[] StatementResponses { get; init; } = [];
    }

    public class StatementResponse
    {
        [XmlElement("BANKACCTFROM")] public BankAccount? BankAccount { get; init; }
        [XmlElement("AVAILBAL")] public BankAccountBalance? AvailableBalance { get; init; }
        [XmlElement("LEDGERBAL")] public BankAccountBalance? LedgerBalance { get; init; }

        [XmlArray("BANKTRANLIST")] [XmlArrayItem("STMTTRN")] public StatementTransaction[] StatementTransactions { get; init; } = [];
    }

    public class BankAccount
    {
        [XmlElement("BANKID")] public string? BankIdentifier { get; init; }
        [XmlElement("ACCTID")] public string? AccountNumber { get; init; }
    }

    public class BankAccountBalance
    {
        [XmlElement("DTASOF")] public string RawDate { get; init; } = null!;
        [XmlElement("BALAMT")] public decimal Amount { get; init; }

        public DateOnly Date => DateOnly.ParseExact(this.RawDate, "yyyyMMddHHmmss", null);
    }

    public class StatementTransaction
    {
        [XmlElement("DTPOSTED")] public string RawDate { get; init; } = null!;
        [XmlElement("TRNAMT")] public decimal Amount { get; init; }
        [XmlElement("FITID")] public string Identifier { get; init; } = null!;
        [XmlElement("NAME")] public string Label { get; init; } = null!;

        public DateOnly Date => DateOnly.ParseExact(this.RawDate, "yyyyMMdd", null);
    }
}