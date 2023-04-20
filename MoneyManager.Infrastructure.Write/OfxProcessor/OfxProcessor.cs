using System.Xml.Serialization;

namespace MoneyManager.Infrastructure.Write.OfxProcessor;

public class OfxProcessor : IOfxProcessor
{
    public Task<AccountId> Parse(Stream stream)
    {
        XmlSerializer serializer = new(typeof(Ofx));
        Ofx root = (Ofx)serializer.Deserialize(stream)!;

        BankAccount? bankAccount = root.Bank?.StatementTransaction?.Statement?.BankAccount;
        if (bankAccount is null)
            throw new CannotProcessOfxContent("Cannot find bank account node (BANKACCTFROM)");
        if (bankAccount.BankIdentifier is null)
            throw new CannotProcessOfxContent("Cannot find bank identifier node (BANKID)");
        if (bankAccount.AccountNumber is null)
            throw new CannotProcessOfxContent("Cannot find account number node (ACCTID)");

        return Task.FromResult(new AccountId(bankAccount.BankIdentifier, bankAccount.AccountNumber));
    }

    public class Ofx
    {
        [XmlElement(ElementName = "BANKMSGSRSV1")]
        public Bank? Bank;
    }

    public class Bank
    {
        [XmlElement(ElementName = "STMTTRNRS")]
        public StatementTransaction? StatementTransaction;
    }

    public class StatementTransaction
    {
        [XmlElement(ElementName = "STMTRS")] public Statement? Statement;
    }

    public class Statement
    {
        [XmlElement(ElementName = "BANKACCTFROM")]
        public BankAccount? BankAccount;
    }

    public class BankAccount
    {
        [XmlElement(ElementName = "BANKID")] public string? BankIdentifier;

        [XmlElement(ElementName = "ACCTID")] public string? AccountNumber;
    }
}