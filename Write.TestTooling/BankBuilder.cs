﻿using Write.App.Model.Banks;

namespace Write.TestTooling;

public record BankBuilder(Guid Id, string ExternalId, string Name)
{
    public static BankBuilder Create() =>
        new(Guid.NewGuid(), "0052158911", "The name");

    public Bank Build() =>
        Bank.From(this.ToSnapshot());

    public BankSnapshot ToSnapshot() =>
        new(this.Id, this.ExternalId);
}