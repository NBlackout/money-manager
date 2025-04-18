﻿namespace Shared.Presentation;

public record AccountDetailsPresentation(Guid Id, string Label, string Number, decimal Balance, DateOnly BalanceDate);