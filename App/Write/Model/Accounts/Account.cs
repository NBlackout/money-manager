﻿using App.Write.Model.Categories;
using App.Write.Model.Transactions;
using App.Write.Model.ValueObjects;

namespace App.Write.Model.Accounts;

public class Account : DomainEntity<AccountId>
{
    private readonly ExternalId externalId;
    private Label label;
    private Balance balance;

    public AccountSnapshot Snapshot => new(this.Id, this.externalId.Value, this.label.Value, this.balance.Amount, this.balance.BalanceDate);

    private Account(AccountId id, ExternalId externalId, Label label, Balance balance) : base(id)
    {
        this.externalId = externalId;
        this.label = label;
        this.balance = balance;
    }

    public static Account StartTracking(AccountId id, ExternalId accountNumber, Balance balance) =>
        new(id, accountNumber, new Label(accountNumber.Value), balance);

    public void Synchronize(Balance newBalance) =>
        this.balance = newBalance;

    public void AssignLabel(Label newLabel) =>
        this.label = newLabel;

    public Transaction AttachTransaction(
        TransactionId transactionId,
        ExternalId externalId,
        Amount amount,
        Label transactionLabel,
        DateOnly date,
        Category? category) =>
        new(transactionId, this.Id, externalId, amount, transactionLabel, date, category);

    public static Account From(AccountSnapshot snapshot) =>
        new(snapshot.Id, new ExternalId(snapshot.Number), new Label(snapshot.Label), new Balance(snapshot.Balance, snapshot.BalanceDate));
}