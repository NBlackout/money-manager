using System.Text.Json;
using App.Read.Ports;
using App.Shared;

namespace App.Tests.Read.TestDoubles;

public class StubbedPeriodPerformanceDataSource : IPeriodPerformanceDataSource
{
    private readonly Dictionary<string, PeriodPerformancePresentation[]> data = [];

    public Task<PeriodPerformancePresentation[]> All(Period[] dateRanges) =>
        Task.FromResult(this.data[Serialize(dateRanges)]);

    public void Feed(Period[] dateRanges, PeriodPerformancePresentation[] expected) =>
        this.data[Serialize(dateRanges)] = expected;

    private static string Serialize(Period[] dateRanges) =>
        JsonSerializer.Serialize(dateRanges);
}