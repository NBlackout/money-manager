﻿using MoneyManager.Application.Write.Ports;
using MoneyManager.Infrastructure.Write.OfxProcessing;
using static MoneyManager.Application.Write.Tests.ImportBankStatementTests.Data;

namespace MoneyManager.Application.Write.Tests;

public class ImportBankStatementTests
{
    private readonly InMemoryAccountRepository repository;
    private readonly StubbedOfxParser ofxParser;
    private readonly ImportBankStatement sut;

    public ImportBankStatementTests()
    {
        this.repository = new InMemoryAccountRepository();
        this.ofxParser = new StubbedOfxParser();
        this.sut = new ImportBankStatement(this.repository, this.ofxParser);
    }

    [Fact]
    public async Task Should_track_unknown_account()
    {
        Guid id = Guid.NewGuid();
        this.ofxParser.SetResultFor(TheStream, new AccountStatement(TheBankIdentifier, TheAccountNumber, TheBalance));
        this.repository.NextId = () => id;

        await this.Verify_ImportTransactions(
            TheStream,
            new AccountSnapshot(id, TheBankIdentifier, TheAccountNumber, TheBalance)
        );
    }

    [Fact]
    public async Task Should_synchronize_already_tracked_account()
    {
        Account existingAccount = AnAccount(TheBankIdentifier, TheAccountNumber);
        ExternalId externalId = new(TheBankIdentifier, TheAccountNumber);
        this.repository.FeedByExternalId(externalId, existingAccount);
        this.ofxParser.SetResultFor(TheStream, new AccountStatement(TheBankIdentifier, TheAccountNumber, 1337.42m));

        await this.Verify_ImportTransactions(
            TheStream,
            new AccountSnapshot(existingAccount.Id, TheBankIdentifier, TheAccountNumber, TheBalance)
        );
    }

    private async Task Verify_ImportTransactions(Stream stream, AccountSnapshot expected)
    {
        await this.sut.Execute(stream);

        Account actual = (await this.repository.GetById(expected.Id));
        actual.Should().BeEquivalentTo(Account.From(expected));
    }

    internal static class Data
    {
        public const string TheBankIdentifier = "Bank";
        public const string TheAccountNumber = "Account";
        public const decimal TheBalance = 1037.66m;

        public static readonly MemoryStream TheStream = new(new byte[] { 0xF0, 0x42 });

        public static Account AnAccount(string bankIdentifier, string number)
        {
            AccountSnapshot accountSnapshot = new(Guid.NewGuid(), bankIdentifier, number, 12.34m);

            return Account.From(accountSnapshot);
        }
    }
}