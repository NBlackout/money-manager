﻿namespace MoneyManager.Client.Read.Application.Ports;

public interface IAccountSummariesGateway
{
    Task<IReadOnlyCollection<AccountSummaryPresentation>> Get();
}