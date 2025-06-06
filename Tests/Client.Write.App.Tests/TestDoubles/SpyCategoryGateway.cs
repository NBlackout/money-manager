﻿using Client.Write.App.Ports;

namespace Client.Write.App.Tests.TestDoubles;

public class SpyCategoryGateway : ICategoryGateway
{
    public List<(Guid, string, string)> CreateCalls { get; } = [];
    public List<Guid> DeleteCalls { get; } = [];

    public Task Create(Guid id, string label, string keywords)
    {
        this.CreateCalls.Add((id, label, keywords));

        return Task.CompletedTask;
    }

    public Task Delete(Guid id)
    {
        this.DeleteCalls.Add(id);

        return Task.CompletedTask;
    }
}