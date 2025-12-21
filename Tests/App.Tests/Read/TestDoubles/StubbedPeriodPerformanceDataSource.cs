using System.Text.Json;
using App.Read.Ports;
using App.Shared;

namespace App.Tests.Read.TestDoubles;

public class StubbedPeriodPerformanceDataSource : IPeriodPerformanceDataSource
{
    private readonly Dictionary<string, PeriodPerformancePresentation[]> data = [];

    public Task<PeriodPerformancePresentation[]> All(Period[] periods) =>
        Task.FromResult(this.data[Serialize(periods)]);

    public void Feed(Period[] periods, PeriodPerformancePresentation[] expected) =>
        this.data[Serialize(periods)] = expected;

    private static string Serialize(Period[] periods) =>
        JsonSerializer.Serialize(periods);
}