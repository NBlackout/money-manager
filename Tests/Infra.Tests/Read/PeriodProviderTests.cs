using App.Read.Ports;
using App.Shared;
using App.Shared.Ports;
using Infra.Read;
using Infra.Tests.Tooling;
using Microsoft.Extensions.DependencyInjection;
using TestTooling.TestDoubles;

namespace Infra.Tests.Read;

public class PeriodProviderTests : InfraTest<IPeriodProvider, PeriodProvider>
{
    private readonly StubbedClock clock = new();

    protected override void Configure(IServiceCollection services) =>
        services.AddSingleton<IClock>(this.clock);

    [Fact]
    public async Task Gives_rolling_twelve_months()
    {
        this.TodayIs(DateOnly.Parse("2025-01-25"));

        await this.Verify_rolling_twelve_months(
            new Period(DateOnly.Parse("2024-02-01"), DateOnly.Parse("2024-02-29")),
            new Period(DateOnly.Parse("2024-03-01"), DateOnly.Parse("2024-03-31")),
            new Period(DateOnly.Parse("2024-04-01"), DateOnly.Parse("2024-04-30")),
            new Period(DateOnly.Parse("2024-05-01"), DateOnly.Parse("2024-05-31")),
            new Period(DateOnly.Parse("2024-06-01"), DateOnly.Parse("2024-06-30")),
            new Period(DateOnly.Parse("2024-07-01"), DateOnly.Parse("2024-07-31")),
            new Period(DateOnly.Parse("2024-08-01"), DateOnly.Parse("2024-08-31")),
            new Period(DateOnly.Parse("2024-09-01"), DateOnly.Parse("2024-09-30")),
            new Period(DateOnly.Parse("2024-10-01"), DateOnly.Parse("2024-10-31")),
            new Period(DateOnly.Parse("2024-11-01"), DateOnly.Parse("2024-11-30")),
            new Period(DateOnly.Parse("2024-12-01"), DateOnly.Parse("2024-12-31")),
            new Period(DateOnly.Parse("2025-01-01"), DateOnly.Parse("2025-01-31"))
        );
    }

    [Fact]
    public async Task Gives_next_three_months()
    {
        this.TodayIs(DateOnly.Parse("2022-08-07"));

        await this.Verify_next_three_months(
            new Period(DateOnly.Parse("2022-09-01"), DateOnly.Parse("2022-09-30")),
            new Period(DateOnly.Parse("2022-10-01"), DateOnly.Parse("2022-10-31")),
            new Period(DateOnly.Parse("2022-11-01"), DateOnly.Parse("2022-11-30"))
        );
    }

    private async Task Verify_rolling_twelve_months(params Period[] expected)
    {
        Period[] actual = await this.Sut.RollingTwelveMonths();
        actual.Should().Equal(expected);
    }

    private async Task Verify_next_three_months(params Period[] expected)
    {
        Period[] actual = await this.Sut.NextThreeMonths();
        actual.Should().Equal(expected);
    }

    private void TodayIs(DateOnly today) =>
        this.clock.Today = today;
}