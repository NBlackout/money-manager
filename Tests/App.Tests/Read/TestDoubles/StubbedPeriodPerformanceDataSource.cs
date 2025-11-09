using System.Text.Json;
using App.Read.Ports;
using App.Shared;

namespace App.Tests.Read.TestDoubles;

public class StubbedPeriodPerformanceDataSource : IPeriodPerformanceDataSource
{
    private readonly Dictionary<string, PeriodPerformancePresentation[]> data = [];

    public Task<PeriodPerformancePresentation[]> All(DateRange[] dateRanges) =>
        Task.FromResult(this.data[Serialize(dateRanges)]);

    public void Feed(DateRange[] dateRanges, PeriodPerformancePresentation[] expected) =>
        this.data[Serialize(dateRanges)] = expected;

    private static string Serialize(DateRange[] dateRanges) =>
        JsonSerializer.Serialize(dateRanges);
}