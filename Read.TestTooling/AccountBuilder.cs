﻿using Shared.Presentation;
using Write.App.Model.Accounts;

namespace Read.TestTooling;

public record AccountBuilder(Guid Id, string Label, string Number, decimal Balance, DateTime BalanceDate)
{
    public Account Build() =>
        Account.From(new AccountSnapshot(this.Id, this.Number, this.Label, this.Balance, this.BalanceDate));

    public AccountSummaryPresentation ToSummary() =>
        new(this.Id, this.Label, this.Number, this.Balance, this.BalanceDate);

    public AccountDetailsPresentation ToDetails() =>
        new(this.Id, this.Label, this.Number, this.Balance, this.BalanceDate);
}
