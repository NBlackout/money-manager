﻿using MoneyManager.Client.Read.Infrastructure.Gateways.Account;

namespace MoneyManager.Client.Read.Application.Tests.UseCases;

public class TransactionsOfMonthTests
{
    private readonly StubbedAccountGateway gateway;
    private readonly TransactionsOfMonth sut;

    public TransactionsOfMonthTests()
    {
        this.gateway = new StubbedAccountGateway();
        this.sut = new TransactionsOfMonth(this.gateway);
    }

    [Fact]
    public async Task Should_retrieve_transactions_of_month()
    {
        Guid accountId = Guid.NewGuid();
        const int year = 1432;
        const int month = 11;
        TransactionSummaryPresentation[] expected =
        {
            new(Guid.NewGuid(), -18.99m, "Money", DateTime.Parse("2020-06-09"), null),
            new(Guid.NewGuid(), -80.00m, "Debit", DateTime.Parse("2013-07-15"), "A category")
        };
        this.gateway.Feed(accountId, year, month, expected);

        IReadOnlyCollection<TransactionSummaryPresentation> actual = await this.sut.Execute(accountId, year, month);
        actual.Should().Equal(expected);
    }
}