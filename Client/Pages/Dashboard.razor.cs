using App.Read.Ports;
using App.Read.UseCases;
using Highsoft.Web.Mvc.Charts;
using Highsoft.Web.Mvc.Charts.Rendering;
using Microsoft.JSInterop;

namespace Client.Pages;

public partial class Dashboard
{
    [Inject] public MonthlyPerformance MonthlyPerformance { get; set; } = null!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

    private PeriodPerformancePresentation[]? monthlyPerformance;

    protected override async Task OnInitializedAsync() =>
        this.monthlyPerformance = await this.MonthlyPerformance.Execute();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.monthlyPerformance?.Length != 0)
        {
            HighchartsRenderer renderer = new(
                new Highcharts
                {
                    Title = new Title { Text = "Monthly performance" },
                    XAxis = [new XAxis { Categories = CategoriesBy(this.monthlyPerformance!), TickWidth = 1 }],
                    YAxis =
                    [
                        new YAxis { Title = new YAxisTitle { Text = "Balance (€)" }, Id = "Balance" },
                        new YAxis { Title = new YAxisTitle { Text = "Performance (€)" }, Id = "Performance", Opposite = true }
                    ],
                    Credits = new Credits { Enabled = false },
                    Series = SeriesBy(this.monthlyPerformance!),
                    Tooltip = new Tooltip { Shared = true },
                    PlotOptions = new PlotOptions
                    {
                        Column = new PlotOptionsColumn { Stacking = PlotOptionsColumnStacking.Normal, PointPadding = 0, GroupPadding = 0 }
                    }
                }
            );

            await this.JsRuntime.InvokeVoidAsync("window.renderChart", "dashboard-chart", renderer.GetJsonOptionsForBlazor());
        }
    }

    private static List<string> CategoriesBy(PeriodPerformancePresentation[] presentation) =>
        presentation.Select(b => b.DateRange.From.ToString("MMMM")).ToList();

    private static List<Series> SeriesBy(PeriodPerformancePresentation[] presentation)
    {
        return
        [
            new LineSeries
            {
                Name = "Balance",
                YAxis = "Balance",
                Data = presentation.Select(sb => new LineSeriesData { Y = decimal.ToDouble(sb.Balance) }).ToList(),
                Tooltip = new LineSeriesTooltip { ValueSuffix = " €" },
                PointPlacementNumber = -0.5,
                ZIndex = 1
            },
            new AreaSeries
            {
                Name = "Revenue",
                YAxis = "Performance",
                Data = presentation.Select(sb => new AreaSeriesData { Y = decimal.ToDouble(sb.Performance.Revenue) }).ToList(),
                Color = "green",
                Tooltip = new AreaSeriesTooltip { ValueSuffix = " €" },
                Opacity = 0.25,
                LineWidth = 0,
                Marker = new AreaSeriesMarker { Enabled = false },
            },
            new AreaSeries
            {
                Name = "Expenses",
                YAxis = "Performance",
                Data = presentation.Select(sb => new AreaSeriesData { Y = decimal.ToDouble(sb.Performance.Expenses) }).ToList(),
                Color = "red",
                Tooltip = new AreaSeriesTooltip { ValueSuffix = " €" },
                Opacity = 0.25,
                Marker = new AreaSeriesMarker { Enabled = false },
                LineWidth = 0,
            },
            new ColumnSeries
            {
                Name = "Net",
                YAxis = "Performance",
                Data = presentation.Select(sb => new ColumnSeriesData { Y = decimal.ToDouble(sb.Performance.Net) }).ToList(),
                Zones = [new ColumnSeriesZone { Color = "red", Value = 0 }, new ColumnSeriesZone { Color = "green" }],
                Tooltip = new ColumnSeriesTooltip { ValueSuffix = " €" },
            }
        ];
    }
}