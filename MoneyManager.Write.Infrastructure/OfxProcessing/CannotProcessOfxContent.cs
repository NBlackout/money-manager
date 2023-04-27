namespace MoneyManager.Write.Infrastructure.OfxProcessing;

public class CannotProcessOfxContent : Exception
{
    private CannotProcessOfxContent(string message) : base(message)
    {
    }

    public static CannotProcessOfxContent DueToMissingBankIdentifierNode() =>
        new("Cannot find bank identifier node (BANKID)");

    public static CannotProcessOfxContent DueToMissingAccountNumberNode() =>
        new("Cannot find account number node (ACCTID)");

    public static CannotProcessOfxContent DueToMissingBalanceNode() =>
        new("Cannot find account number node (BALAMT)");
}