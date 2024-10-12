using System.Globalization;

namespace Write.Infra.BankStatementParsing;

public class CsvBankStatementParser
{
    private const char ColumnSeparator = ';';
    private const int TransactionDateIndex = 0;
    private const int TransactionLabelIndex = 2;
    private const int TransactionCategoryIndex = 4;
    private const int TransactionAmountIndex = 5;
    private const int AccountNumberIndex = 7;
    private const int AccountBalanceIndex = 9;

    public async Task<AccountStatement> ExtractAccountStatement(Stream stream)
    {
        List<string> lines = await ReadLinesFrom(stream);

        List<TransactionStatement> transactions = [];
        string? accountNumber = null;
        decimal? balance = null;

        foreach (string line in lines.Skip(1))
        {
            string[] columns = line.Split(ColumnSeparator);

            accountNumber ??= columns[AccountNumberIndex];
            balance = DecimalOrDefault(columns[AccountBalanceIndex]) ?? balance;

            string transactionIdentifier = NextIdentifierUsing(transactions);
            transactions.Add(TransactionFrom(transactionIdentifier, columns));
        }

        return AccountFrom(accountNumber!, balance!.Value, transactions.ToArray());
    }

    private static async Task<List<string>> ReadLinesFrom(Stream stream)
    {
        List<string> lines = [];

        using StreamReader reader = new(stream);
        while (!reader.EndOfStream)
            lines.Add((await reader.ReadLineAsync())!.Replace("\"", string.Empty));

        return lines;
    }

    private static string NextIdentifierUsing(List<TransactionStatement> transactions) =>
        $"{transactions.Count + 1}";

    private static TransactionStatement TransactionFrom(string identifier, string[] columns)
    {
        decimal amount = ParseDecimal(columns[TransactionAmountIndex]);
        string label = columns[TransactionLabelIndex];
        DateTime date = DateTime.Parse(columns[TransactionDateIndex]);
        string category = columns[TransactionCategoryIndex];

        return new TransactionStatement(identifier, amount, label, date, category);
    }

    private static AccountStatement AccountFrom(string number, decimal balance, TransactionStatement[] transactions) =>
        new(number, balance, DateTime.Parse("2000-01-01"), transactions.ToArray());

    private static decimal? DecimalOrDefault(string value) =>
        string.IsNullOrEmpty(value) is false ? ParseDecimal(value) : null;

    private static decimal ParseDecimal(string value) =>
        decimal.Parse(value.Replace(",", ".").Replace(" ", string.Empty), CultureInfo.InvariantCulture);
}