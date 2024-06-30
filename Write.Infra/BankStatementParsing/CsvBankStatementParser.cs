using System.Globalization;

namespace Write.Infra.BankStatementParsing;

public class CsvBankStatementParser
{
    public async Task<AccountStatement> ExtractAccountStatement(Stream stream)
    {
        List<string> lines = await LinesFrom(stream);

        List<TransactionStatement> transactions = [];
        string? accountNumber = null;
        decimal? balance = null;

        foreach (string line in lines.Skip(1))
        {
            string[] columns = line.Split(';');

            if (accountNumber == null)
            {
                accountNumber = columns[7];
                balance = ParseDecimal(columns[9]);
            }

            string transactionIdentifier = (transactions.Count + 1).ToString();
            transactions.Add(TransactionFrom(transactionIdentifier, columns));
        }

        return AccountFrom(accountNumber!, balance!.Value, transactions.ToArray());
    }

    private static async Task<List<string>> LinesFrom(Stream stream)
    {
        using StreamReader reader = new(stream);
        List<string> lines = [];

        while (!reader.EndOfStream)
            lines.Add((await reader.ReadLineAsync())!.Replace("\"", string.Empty));
        return lines;
    }

    private static TransactionStatement TransactionFrom(string identifier, string[] columns)
    {
        decimal amount = ParseDecimal(columns[5]);
        string label = columns[2];
        DateTime date = DateTime.Parse(columns[0]);
        string category = columns[4];

        return new TransactionStatement(identifier, amount, label, date, category);
    }

    private static AccountStatement AccountFrom(string number, decimal balance, TransactionStatement[] transactions) =>
        new(number, balance, DateTime.Parse("2000-01-01"), transactions.ToArray());

    private static decimal ParseDecimal(string value) =>
        decimal.Parse(value.Replace(",", ".").Replace(" ", string.Empty), CultureInfo.InvariantCulture);
}
