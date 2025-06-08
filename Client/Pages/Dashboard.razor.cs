using Highsoft.Web.Mvc.Charts;
using Highsoft.Web.Mvc.Charts.Rendering;
using Microsoft.JSInterop;

namespace Client.Pages;

public partial class Dashboard
{
    [Inject] public SlidingBalances SlidingBalances { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        SlidingBalancesPresentation presentation = await this.SlidingBalances.Execute();
        HighchartsRenderer renderer = new(
            new Highcharts
            {
                Title = new Title { Text = "Sliding balances" },
                XAxis = [new XAxis { Categories = CategoriesBy(presentation) }],
                Credits = new Credits { Enabled = false },
                Series = SeriesBy(presentation)
            }
        );

        await this.JsRuntime.InvokeVoidAsync(
            "window.renderChart",
            "dashboard-chart",
            renderer.GetJsonOptionsForBlazor()
        );
    }

    private static List<string> CategoriesBy(SlidingBalancesPresentation presentation) =>
        presentation.AccountBalancesByDate.Select(b => b.BalanceDate.ToShortDateString()).ToList();

    private static List<Series> SeriesBy(SlidingBalancesPresentation presentation) =>
        presentation
            .AccountBalancesByDate
            .SelectMany(d => d.AccountBalances.Select(b => b.AccountLabel))
            .Distinct()
            .Select(a => SeriesBy(presentation, a))
            .ToList();

    private static Series SeriesBy(SlidingBalancesPresentation presentation, string accountLabel) =>
        new AreaSeries
        {
            Name = accountLabel,
            Data = presentation.AccountBalancesByDate.Select(b => AreaSeriesDataBy(accountLabel, b)).ToList()
        };

    private static AreaSeriesData AreaSeriesDataBy(string accountLabel, SlidingBalancePresentation date) =>
        new() { Y = decimal.ToDouble(date.AccountBalances.Single(b => b.AccountLabel == accountLabel).Balance) };
}