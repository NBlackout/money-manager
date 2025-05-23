﻿using Client.Write.App.Ports;

namespace Client.Write.App.Tests.TestDoubles;

public class SpyAccountGateway : IAccountGateway
{
    public List<Guid> StopTrackingCalls { get; } = [];
    public List<Guid> ResumeTrackingCalls { get; } = [];
    public List<(Guid, string)> AssignLabelCalls { get; } = [];

    public Task StopTracking(Guid id)
    {
        this.StopTrackingCalls.Add(id);

        return Task.CompletedTask;
    }

    public Task ResumeTracking(Guid id)
    {
        this.ResumeTrackingCalls.Add(id);

        return Task.CompletedTask;
    }

    public Task AssignLabel(Guid id, string label)
    {
        this.AssignLabelCalls.Add((id, label));

        return Task.CompletedTask;
    }
}