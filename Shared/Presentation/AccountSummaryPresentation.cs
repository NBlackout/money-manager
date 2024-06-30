﻿namespace Shared.Presentation;

public record AccountSummaryPresentation(Guid Id, string Label, string Number, decimal Balance, DateTime BalanceDate);
