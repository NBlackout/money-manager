﻿using System.Globalization;
using App.Write.Model.Accounts;
using App.Write.Model.ValueObjects;
using App.Write.Ports;

namespace Infra.Write.BankStatementParsing;

public class CsvBankStatementParser
{
    public async Task<AccountStatement> ExtractAccountStatement(Stream stream)
    {
        List<string> lines = await ReadLinesFrom(stream);
        BankStatementRow[] rows = [..lines.Skip(1).Select(BankStatementRow.From)];

        string accountNumber = rows.First().AccountNumber;
        BankStatementRow firstRowWithBalance = rows.First(r => r.AccountBalance.HasValue);
        decimal accountBalance = firstRowWithBalance.AccountBalance!.Value;
        TransactionStatement[] transactions = [..rows.Select(r => r.ToTransactionStatement())];

        return new AccountStatement(new ExternalId(accountNumber), new Balance(accountBalance, firstRowWithBalance.TransactionDate), transactions);
    }

    private static async Task<List<string>> ReadLinesFrom(Stream stream)
    {
        List<string> lines = [];

        using StreamReader reader = new(stream);
        while (!reader.EndOfStream)
            lines.Add((await reader.ReadLineAsync())!.Replace("\"", string.Empty));

        return lines;
    }

    private record BankStatementRow(
        int RowNumber,
        DateOnly TransactionDate,
        string TransactionLabel,
        string TransactionCategory,
        decimal TransactionAmount,
        string AccountNumber,
        decimal? AccountBalance
    )
    {
        private const char ColumnSeparator = ';';

        private const int TransactionDateIndex = 0;
        private const int TransactionLabelIndex = 2;
        private const int TransactionCategoryIndex = 4;
        private const int TransactionAmountIndex = 6;
        private const int AccountNumberIndex = 8;
        private const int AccountBalanceIndex = 10;

        public TransactionStatement ToTransactionStatement() =>
            new(
                new ExternalId(this.AccountNumber + "_" + this.RowNumber),
                new Amount(this.TransactionAmount),
                new Label(this.TransactionLabel),
                this.TransactionDate,
                new Label(this.TransactionCategory)
            );

        public static BankStatementRow From(string line, int lineNumber)
        {
            string[] columns = line.Split(ColumnSeparator);

            return new BankStatementRow(
                lineNumber + 1,
                DateOnly.Parse(columns[TransactionDateIndex]),
                columns[TransactionLabelIndex],
                columns[TransactionCategoryIndex],
                ParseDecimal(columns[TransactionAmountIndex]),
                columns[AccountNumberIndex],
                ParseDecimalOrDefault(columns[AccountBalanceIndex])
            );
        }

        private static decimal? ParseDecimalOrDefault(string value) =>
            string.IsNullOrEmpty(value) is false ? ParseDecimal(value) : null;

        private static decimal ParseDecimal(string value) =>
            decimal.Parse(value.Replace(",", ".").Replace(" ", string.Empty).Replace("\"", string.Empty), CultureInfo.InvariantCulture);
    }
}