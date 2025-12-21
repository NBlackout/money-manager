using System.Text.Json;
using App.Read.Ports;
using App.Shared;

namespace App.Tests.Read.TestDoubles;

public class StubbedSamplePeriodsDataSource : ISamplePeriodsDataSource
{
    private readonly Dictionary<string, SamplePeriod[]> data = [];

    public Task<SamplePeriod[]> Of(Period[] periods) =>
        Task.FromResult(this.data[Serialize(periods)]);

    public void Feed(Period[] periods, SamplePeriod[] samplePeriods) =>
        this.data[Serialize(periods)] = samplePeriods;

    private static string Serialize(Period[] periods) =>
        JsonSerializer.Serialize(periods);
}