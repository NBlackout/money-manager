﻿using MoneyManager.Read.Application.UseCases;
using MoneyManager.Read.Infrastructure.DataSources.AccountDetails;
using MoneyManager.Shared.Presentation;

namespace MoneyManager.Read.Application.Tests.UseCases;

public class AccountDetailsTests
{
    private readonly StubbedAccountDetailsDataSource dataSource;
    private readonly AccountDetails sut;

    public AccountDetailsTests()
    {
        this.dataSource = new StubbedAccountDetailsDataSource();
        this.sut = new AccountDetails(this.dataSource);
    }

    [Fact]
    public async Task Should_retrieve_account_details()
    {
        AccountDetailsPresentation expected = new(Guid.NewGuid(), "Account label", 14.07m);
        this.dataSource.Feed(expected.Id, expected);

        AccountDetailsPresentation actual = await this.sut.Execute(expected.Id);

        actual.Should().Be(expected);
    }
}