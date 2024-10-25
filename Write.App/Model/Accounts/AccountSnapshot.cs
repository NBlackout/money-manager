﻿namespace Write.App.Model.Accounts;

public record AccountSnapshot(AccountId Id, string Number, string Label, decimal BalanceAmount, DateOnly BalanceDate);